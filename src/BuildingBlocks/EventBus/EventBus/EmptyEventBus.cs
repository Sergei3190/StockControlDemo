using EventBus.Events;
using EventBus.Interfaces;

using Microsoft.Extensions.Logging;

namespace EventBus;

/// <summary>
/// Затычка для выключенной шины
/// </summary>
public class EmptyEventBus : IEventBus
{
    private readonly ILogger<EmptyEventBus> _logger;

    public EmptyEventBus(ILogger<EmptyEventBus> logger)
    {
        _logger = logger;
    }

    public void Publish(IntegrationEvent @event)
    {
        _logger.LogInformation("Publish event {@event}", @event);
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        _logger.LogInformation("Subscribe");
    }

    public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
    {
        _logger.LogInformation("SubscribeDynamic {eventName}", eventName);
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        _logger.LogInformation("Unsubscribe");
    }

    public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
    {
        _logger.LogInformation("UnsubscribeDynamic {eventName}", eventName);
    }
}