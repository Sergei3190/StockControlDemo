using EventBus.Events;
using EventBus.Interfaces;

using EventBusRabbitMQ.Interfaces;

using Microsoft.Extensions.Logging;

namespace EventBusRabbitMQ;

/// <summary>
/// Затычка для выключенной шины
/// </summary>
public class EmptyEventBusDeadLetter : IEventBusDeadLetter
{
    private readonly ILogger<EmptyEventBusDeadLetter> _logger;

    public EmptyEventBusDeadLetter(ILogger<EmptyEventBusDeadLetter> logger)
    {
        _logger = logger;
        Headers = new Dictionary<string, object>(); 
    }

    public IDictionary<string, object> Headers { get; set; }

    public void StartDeadLetter(CancellationToken cancel)
    {
        _logger.LogInformation("StartDeadLetter {cancel}", cancel);
    }

    public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        _logger.LogInformation("Subscribe");
    }

    public void Unsubscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
    {
        _logger.LogInformation("Unsubscribe");
    }
}