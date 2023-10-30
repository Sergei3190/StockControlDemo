using MediatR;

namespace StockControl.API.Domain.Events.Moving;

/// <summary>
/// Доменное событие создания перемещения
/// </summary>
public record MovingCreatedDomainEvent(Guid MovingId) : INotification
{
}
