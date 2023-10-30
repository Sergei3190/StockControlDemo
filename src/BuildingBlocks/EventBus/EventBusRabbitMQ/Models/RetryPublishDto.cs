using RabbitMQ.Client;

namespace EventBusRabbitMQ.Models;

/// <summary>
/// Модель данных для повторной публикации 
/// </summary>
internal class RetryPublishDto
{
    public Guid EventId { get; }
    public ReadOnlyMemory<byte> Body { get; }
    public byte[]? Exchange { get; }
    public byte[]? EventName { get; }
    public IBasicProperties? Properties { get; }

    public RetryPublishDto(
        Guid @eventId,
        ReadOnlyMemory<byte> body,
        byte[]? exchange,
        byte[]? eventName,
        IBasicProperties? properties)
    {
        EventId = @eventId;
        Body = body;
        Exchange = exchange;
        EventName = eventName;
        Properties = properties;
    }
}