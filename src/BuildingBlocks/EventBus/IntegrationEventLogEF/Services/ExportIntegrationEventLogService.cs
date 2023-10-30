using System.Data.Common;
using System.Reflection;

using EventBus.Events;

using IntegrationEventLogEF.Entities;
using IntegrationEventLogEF.Services.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationEventLogEF;

public class ExportIntegrationEventLogService : IExportIntegrationEventLogService, IDisposable
{
	private readonly IntegrationEventLogContext _integrationEventLogContext;
	private readonly DbConnection _dbConnection;
	private readonly List<Type> _eventTypes;
	private volatile bool _disposedValue;

	public ExportIntegrationEventLogService(DbConnection dbConnection)
	{
		_dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

		// передаём строку подключения бд микросервиса
		_integrationEventLogContext = new IntegrationEventLogContext(
			new DbContextOptionsBuilder<IntegrationEventLogContext>()
				.UseSqlServer(_dbConnection)
				.Options);

		// получаем все интеграционноые события, зарегестрированные в исполняемой сборке, те в той сборке, где у нас зарегестрирован данный сервис,
		// а также в обмем сервисе Service.Common, тк у нас там храняться основные интеграционные события
		_eventTypes = Assembly.Load(Assembly.GetEntryAssembly()!.FullName!)
			.GetTypes()
			.Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
			.Union(Assembly.Load("Service.Common").GetTypes().Where(t => t.Name.EndsWith(nameof(IntegrationEvent))))
			.ToList();
	}

	public async Task<IEnumerable<ExportIntegrationEventLog>> GetEventLogsAsync(EventLogFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _integrationEventLogContext.ExportIntegrationEventLogs.AsQueryable();

		if (filter.States != null)
		{
			var statesToString = filter.States.Cast<EventStateEnum>().Select(s => s.ToString());
			query = query.Where(q => statesToString.Contains(q.State));
		}

		if (filter.MaxTimeSent > 0)
			query = query.Where(q => q.TimesSent <= filter.MaxTimeSent);

		var result = await query
			.Skip(filter.Skip)
			.Take(filter.Take)
			.ToListAsync()
			.ConfigureAwait(false);

		if (result.Any())
		{
			return result
				.OrderBy(o => o.CreationTime)
				.Select(e => e.DeserializeJsonContent(_eventTypes.Find(t => t.Name == e.EventTypeShortName)!));
		}

		return new List<ExportIntegrationEventLog>();
	}

	public async Task<ExportIntegrationEventLog?> GetEventLogByEventIdAsync(Guid eventId)
	{
		return await _integrationEventLogContext.ExportIntegrationEventLogs
			.Where(e => e.EventId == eventId)
			.SingleOrDefaultAsync()
			.ConfigureAwait(false);
	}

	public async Task<int> GetCountAsync(params EventStateEnum[] states)
	{
		var query = _integrationEventLogContext.ExportIntegrationEventLogs.AsQueryable();

		if (states != null)
		{
			var statesToString = states.Cast<EventStateEnum>().Select(s => s.ToString());
			query = query.Where(q => statesToString.Contains(q.State));
		}

		return await query
			.CountAsync()
			.ConfigureAwait(false);
	}

	public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction)
	{
		if (transaction == null)
			throw new ArgumentNullException(nameof(transaction));

		var eventLogEntry = new ExportIntegrationEventLog(@event, transaction.TransactionId);

		// для сохранения используем переданную из подписчика/микросервиса транзакцию
		_integrationEventLogContext.Database.UseTransaction(transaction.GetDbTransaction());
		_integrationEventLogContext.ExportIntegrationEventLogs.Add(eventLogEntry);

		// возвращаем выполнение в вызывающий контекст
		return _integrationEventLogContext.SaveChangesAsync();
	}

	public async Task MarkEventAsPublishedAsync(Guid eventId)
	{
		await UpdateEventStatusAsync(eventId, EventStateEnum.Published);
	}

	public async Task MarkEventAsInProgressAsync(Guid eventId)
	{
		await UpdateEventStatusAsync(eventId, EventStateEnum.InProgress);
	}

	public async Task MarkEventAsFailedAsync(Guid eventId, string? error = null)
	{
		await UpdateEventStatusAsync(eventId, EventStateEnum.PublishedFailed, error);
	}

	private async Task UpdateEventStatusAsync(Guid eventId, EventStateEnum status, string? error = null)
	{
		var eventLogEntry = await _integrationEventLogContext.ExportIntegrationEventLogs
			.SingleAsync(ie => ie.EventId == eventId)
			.ConfigureAwait(false);

		eventLogEntry.State = status.ToString();
		eventLogEntry.Error = error;

		if (status == EventStateEnum.InProgress)
			eventLogEntry.TimesSent++;

		_integrationEventLogContext.ExportIntegrationEventLogs.Update(eventLogEntry);

		await _integrationEventLogContext.SaveChangesAsync();
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposedValue)
		{
			if (disposing)
			{
				_integrationEventLogContext?.Dispose();
			}

			_disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
