using EventBusRabbitMQ.Interfaces;

namespace Service.Common.Integration.Settings;

/// <summary>
/// Конфигурация брокера сообщений
/// </summary>
public class RabbitMQBrokerConfig : IRabbitMQBrokerConfig
{
    public string Broker { get; init; }
    public string ExchangeType { get; init; }
    public string QueueName { get; set; }
    public int PrefetchCount { get; init; }
    public int RetryCount { get; init; }

    public RabbitMQBrokerConfig()
    {

    }

    public RabbitMQBrokerConfig(string brokerName, string exchangeType, string queueName, int prefetchCount, int retryCount)
    {
        Broker = brokerName;
        ExchangeType = exchangeType;
        QueueName = queueName;
        PrefetchCount = prefetchCount;
        RetryCount = retryCount;
    }
}