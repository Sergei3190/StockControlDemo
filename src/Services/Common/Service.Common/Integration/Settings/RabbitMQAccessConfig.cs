using EventBusRabbitMQ.Interfaces;

namespace Service.Common.Integration.Settings;

/// <summary>
/// Конфигурация доступа к RabbitMQ
/// </summary>
public class RabbitMQAccessConfig : IRabbitMQAccessConfig
{
    public string DockerHost { get; init; }
    public string Host { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    public int RetryCount { get; init; }

    public RabbitMQAccessConfig()
    {

    }

    public RabbitMQAccessConfig(string dockerHost, string host, string userName, string password, int retryCount)
    {
		DockerHost = dockerHost;
		Host = host;
        UserName = userName;
        Password = password;
        RetryCount = retryCount;
    }
}