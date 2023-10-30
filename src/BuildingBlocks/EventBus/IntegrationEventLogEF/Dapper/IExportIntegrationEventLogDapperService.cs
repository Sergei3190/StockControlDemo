using IntegrationEventLogEF.Entities;

namespace IntegrationEventLogEF.Dapper;

/// <summary>
/// Сервис журнала исходящих интеграционных событий с применением Dapper
/// </summary>
public interface IExportIntegrationEventLogDapperService
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
