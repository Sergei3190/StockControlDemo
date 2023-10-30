namespace StockControl.API.BackgroundTasks.Settings;

public class EventPublisherSettings : BaseSettings
{
    /// <summary>
    /// Номер блока выборки, каждая выборка имеет размер PageSize
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Размер блока выборки
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Кол-во повторных попыток подключения к БД, политика устойчивости, которая объявлена при создании контекста БД (ConfigureSqlOptions) 
    /// не срабатывает, если БД, для которой она задана, не существует
    /// </summary>
    public int RetryCount { get; set; }

    /// <summary>
    /// Таймаут в секундах между повторными попытками
    /// </summary>
    public int RetryTimeout { get; set; }

    /// <summary>
    /// Максимальное кол-во отправок, после которых сообщение больше не будет публиковаться, те нужно разбираться в чем дело, а затем после исправления ошибки
    /// ставить вновь в очередь на публикацию, например выполнив sql скрипт для обновления TimesSent
    /// </summary>
    public int MaxTimesSent { get; set; }
}