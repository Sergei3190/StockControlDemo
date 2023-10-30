namespace IntegrationEventLogEF;

/// <summary>
/// Сервис журнала входящих успешно обработанных интеграционных событий
/// </summary>
public interface IImportSuccessIntegrationEventLogService : IIntegrationEventLogService
{
    /// <summary>
    /// Метод проверки существования успешно обработанного интеграционного события
    /// </summary>
    Task<bool> CheckIsExistEventLogAsync(EventLogDto eventLogDto);
}