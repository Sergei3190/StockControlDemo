using EventBus.Events;

namespace Service.Common.Integration.Events.Base;

/// <summary>
/// Базовый класс событий RabbitMQ микросервисов приложения StockControl
/// </summary>
public class SCRMQIntegrationEvent : IntegrationEvent
{
    /// <summary>
    /// Идентификатор события, которое было направлено через RabbitMQ
    /// </summary>
    public Guid SourceId { get; }

    public SCRMQIntegrationEvent()
    {
        SourceId = Guid.Parse("73F19E6B-73AD-462B-AC5D-0F3464815314"); // ссылается на builder.ToTable("sources", "app");
    }
}
