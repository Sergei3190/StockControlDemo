using System.Reflection;
using System.Text;

using Dapper;

using EventBus.Events;

using IntegrationEventLogEF.Entities;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace IntegrationEventLogEF.Dapper;

public class ExportIntegrationEventLogDapperService : IExportIntegrationEventLogDapperService
{
	private readonly IEnumerable<Type> _eventTypes;
	private readonly string _connectionString;

	public ExportIntegrationEventLogDapperService(string connectionString)
	{
		_connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

		// получаем все интеграционные события, зарегистрированные в исполняемой сборке, те в той сборке, где у нас зарегистрирован данный сервис,
		// а также в общем сервисе Service.Common, тк у нас там хранятся интеграционные события
		_eventTypes = Assembly.Load(Assembly.GetEntryAssembly()!.FullName!)
			.GetTypes()
			.Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
			.Union(Assembly.Load("Service.Common").GetTypes().Where(t => t.Name.EndsWith(nameof(IntegrationEvent))))
			.ToList();
	}

	public async Task<ExportIntegrationEventLog> GetEventLogByEventIdAsync(Guid eventId)
	{
		using (var connect = new SqlConnection(_connectionString))
		{
			var queryResult = await connect.QueryAsync<ExportIntegrationEventLog>(
				@"select * 
                  from [logger].[export_integration_event_log]
                  where event_id = @event_id",
				new { eventId }).ConfigureAwait(false);

			return queryResult.First();
		}
	}

	public async Task<int> GetCountAsync(EventStateEnum[] states)
	{
		var statesToString = states.Cast<EventStateEnum>().Select(s => s.ToString());

		using (var connect = new SqlConnection(_connectionString))
		{
			var queryResult = await connect.QueryAsync<int>(
				@"select count(*) 
                  from [logger].[export_integration_event_log]
                  where state in @statesToString",
				new { statesToString }).ConfigureAwait(false);
			return queryResult.First();
		}
	}

	public async Task<IEnumerable<ExportIntegrationEventLog>> GetEventLogsAsync(EventLogFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		using (var connect = new SqlConnection(_connectionString))
		{
			var query = new StringBuilder();

			IEnumerable<string> statesToString = null!;

			query.Append(@"select 
				id,
                event_id as 'EventId',
                creation_time as 'CreationTime',
                event_type_name as 'EventTypeName',
                transaction_id as 'TransactionId',
				content,
				times_sent as 'TimesSent',
                state,
                error
				from [logger].[export_integration_event_log] ");

			if (filter.States != null)
			{
				statesToString = filter.States.Cast<EventStateEnum>().Select(s => s.ToString());
				query.Append(@"where state in @statesToString ");
			}

			if (filter.MaxTimeSent > 0)
				query.Append(@"and times_sent <= @MaxTimeSent");

			query
				.AppendLine(@"")
				.AppendLine(@"order by creation_time")
				.AppendLine(@"offset @Skip rows")
				.AppendLine(@"fetch next @Take rows ONLY");

			var queryResult = await connect.QueryAsync<ExportIntegrationEventLog>(query.ToString(), new
			{
				statesToString,
				filter.MaxTimeSent,
				filter.Skip,
				filter.Take,
			})
				.ConfigureAwait(false);

			if (queryResult.Any())
			{
				return queryResult
					.Select(e => e.DeserializeJsonContent(_eventTypes.First(t => t.Name == e.EventTypeShortName)!));
			}

			return new List<ExportIntegrationEventLog>();
		}
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

	private async Task UpdateEventStatusAsync(Guid eventId, EventStateEnum state, string? error = null)
	{
		using (var connect = new SqlConnection(_connectionString))
		{
			var query = new StringBuilder();

			query.AppendLine(@"update [logger].[export_integration_event_log] ");
			query.AppendLine(@"set state = @state, ");
			query.AppendLine(@"error = @error ");

			if (state == EventStateEnum.InProgress)
				query.AppendLine(@", times_sent += 1 ");

			query.AppendLine(@"where event_id = @eventId ");

			await connect.ExecuteAsync(query.ToString(), new
			{
				state,
				error,
				eventId
			})
				.ConfigureAwait(false);
		}
	}
}
