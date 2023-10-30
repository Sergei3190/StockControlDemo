using System.Data.Common;

using EventBus.Events;

using IntegrationEventLogEF.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationEventLogEF;

public class ImportSuccessIntegrationEventLogService : IImportSuccessIntegrationEventLogService, IDisposable
{
    private readonly IntegrationEventLogContext _integrationEventLogContext;
    private readonly DbConnection _dbConnection;
    private volatile bool _disposedValue;

    public ImportSuccessIntegrationEventLogService(DbConnection dbConnection)
    {
        _dbConnection = dbConnection ?? throw new ArgumentNullException(nameof(dbConnection));

		// передаём строку подключения бд микросервиса
		_integrationEventLogContext = new IntegrationEventLogContext(
            new DbContextOptionsBuilder<IntegrationEventLogContext>()
                .UseSqlServer(_dbConnection)
                .Options);
    }

    public async Task<bool> CheckIsExistEventLogAsync(EventLogDto eventLogDto)
    {
        ArgumentNullException.ThrowIfNull(eventLogDto, nameof(eventLogDto));

        // если мы уже обрабатывали пришедшее к нам событие или мы уже обработали событие такого же типа с датой создания позже, то возвращаем истину и говорим
        // что не хотим его обрабатывать
        var result = await _integrationEventLogContext.ImportSuccessIntegrationEventLogs
            .Where(i => i.EventId == eventLogDto.EventId || (i.EventTypeName == eventLogDto.EventTypeName && i.CreationTime > eventLogDto.CreationTime))
            .AnyAsync()
            .ConfigureAwait(false);

        return result;
    }

    public Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction)
    {
        if (transaction == null)
            throw new ArgumentNullException(nameof(transaction));

        var eventLogEntry = new ImportSuccessIntegrationEventLog(@event, transaction.TransactionId);

        // для сохранения используем переданную из подписчика/микросервиса транзакцию
        _integrationEventLogContext.Database.UseTransaction(transaction.GetDbTransaction());
        _integrationEventLogContext.ImportSuccessIntegrationEventLogs.Add(eventLogEntry);

        // возвращаем выполнение в вызывающий контекст
        return _integrationEventLogContext.SaveChangesAsync();
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
