namespace IntegrationEventLogEF;

/// <summary>
/// Статусы события
/// </summary>
public enum EventStateEnum
{
    /// <summary>
    /// Не опубликовано
    /// </summary>
    NotPublished = 0,

    /// <summary>
    /// В прцессе публикации
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Опубликовано, те обработчик на стороне подписчика/микросервиса успешно обработал событие
    /// </summary>
    Published = 2,

    /// <summary>
    /// Публикация завершилась неудачно, стоит отправить заново
    /// </summary>
    PublishedFailed = 3
}