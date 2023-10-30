using RabbitMQ.Client;

namespace EventBusRabbitMQ.Interfaces;

/// <summary>
/// Абстракция менеджера поддерживаемых соединений
/// </summary>
public interface IRabbitMQPersistentConnection : IDisposable
{
    bool IsConnected { get; }

    bool TryConnect();

    IModel CreateModel();
}
