using MediatR;

namespace StockControl.API.Domain.Events.Moving;

/// <summary>
/// Доменное событие обновления перемещения
/// </summary>
public record MovingUpdatedDomainEvent(Guid MovingId) : INotification
{
}
