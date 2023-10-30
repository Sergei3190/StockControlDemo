using EventBusRabbitMQ.Interfaces;

namespace Service.Common.Integration.Settings;

/// <summary>
/// Конфигурация брокера недоставленных сообщений
/// </summary>
public class RabbitMQDlxBrokerConfig : RabbitMQBrokerConfig, IRabbitMQDlxBrokerConfig
{
    public bool Enabled { get; set; }

    public int XDelay { get; set; }

    public RabbitMQDlxBrokerConfig()
    {

    }

    public RabbitMQDlxBrokerConfig(string brokerName, string exchangeType, string queueName, int prefetchCount, int retryCount) :
        base(brokerName, exchangeType, queueName, prefetchCount, retryCount)
    {

    }
}