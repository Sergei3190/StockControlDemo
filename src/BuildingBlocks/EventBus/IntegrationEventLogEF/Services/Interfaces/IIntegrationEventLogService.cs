using EventBus.Events;

using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationEventLogEF;

/// <summary>
/// Базовый сервис журнала интеграционных событий
/// </summary>
public interface IIntegrationEventLogService
{
    /// <summary>
    /// Сохранить событие в журнал событий
    /// </summary>
    Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);
}