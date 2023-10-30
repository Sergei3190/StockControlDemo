using EventBus.Events;
using IntegrationEventLogEF.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationEventLogEF.Services.Interfaces;

/// <summary>
/// Сервис журнала исходящих интеграционных событий
/// </summary>
public interface IExportIntegrationEventLogService
{
    /// <summary>
    /// Кол-во интеграционных событий
    /// </summary>
    Task<int> GetCountAsync(params EventStateEnum[] states);

    /// <summary>
    /// Получить журналы событий
    /// </summary>
    Task<IEnumerable<ExportIntegrationEventLog>> GetEventLogsAsync(EventLogFilterDto filter);

    /// <summary>
    /// Получить журнал события по идентификатору события
    /// </summary>
    Task<ExportIntegrationEventLog> GetEventLogByEventIdAsync(Guid eventId);

    /// <summary>
    /// Сохранить событие в журнал событий
    /// </summary>
    Task SaveEventAsync(IntegrationEvent @event, IDbContextTransaction transaction);

    /// <summary>
    /// Изменить статус события на опубликованное
    /// </summary>
    Task MarkEventAsPublishedAsync(Guid eventId);

    /// <summary>
    /// Изменить статус события на опубликованное
    /// </summary>
    Task MarkEventAsInProgressAsync(Guid eventId);

    /// <summary>
    /// Изменить статус события на неуспешно опубликовано
    /// </summary>
    Task MarkEventAsFailedAsync(Guid eventId, string? error = null);
}