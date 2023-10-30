using EventBus.Events;

namespace EventBus.Interfaces;

/// <summary>
/// Обработчик событий, приходящих из шины
/// </summary>
public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent @event);
}

public interface IIntegrationEventHandler
{
}
