namespace EventBusRabbitMQ.Interfaces;

/// <summary>
/// Конфигурация брокера недоставленных сообщений
/// </summary>
public interface IRabbitMQDlxBrokerConfig : IRabbitMQBrokerConfig
{
    /// <summary>
    /// Включена обменник или нет
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// Задержка в секундах при отправке собщения в очередь недоставленных сообщений
    /// </summary>
    public int XDelay { get; set; }

    /// <summary>
    /// Наименование ключа для обменника недоставленных сообщений, по умолчанию DlxIntegrationEvent
    /// </summary>
    string DlxRoutingKey => "DlxIntegrationEvent";
}