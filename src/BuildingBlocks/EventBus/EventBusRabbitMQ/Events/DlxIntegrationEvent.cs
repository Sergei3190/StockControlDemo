using EventBus.Events;

namespace EventBusRabbitMQ.Events;

/// <summary>
/// Недоставленное событие RabbitMQ
/// </summary>
public class DlxIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// Идентификатор недоставленного события
    /// </summary>
    public Guid EventId { get; }

    public DlxIntegrationEvent(Guid retryEventId)
    {
        EventId = retryEventId;
    }
}
