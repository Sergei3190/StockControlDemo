namespace EventBusRabbitMQ.Interfaces;

/// <summary>
/// Конфигурация брокера сообщений
/// </summary>
public interface IRabbitMQBrokerConfig
{
    /// <summary>
    /// Имя брокера/обменника сообщениями
    /// </summary>
    string Broker { get; init; }

    /// <summary>
    /// Тип брокера/обменника сообщениями
    /// </summary>
    string ExchangeType { get; init; }

    /// <summary>
    /// Имя очереди, через которую будут проходить сообщения
    /// </summary>
    string QueueName { get; set; }

    /// <summary>
    /// Кол-во предварительно выбранных сообщений из очереди для передачи подписчику/микросервису
    /// </summary>
    int PrefetchCount { get; init; }

    /// <summary>
    /// Кол-во повторных попыток публикации сообщения(й)
    /// </summary>
    int RetryCount { get; init; }
}