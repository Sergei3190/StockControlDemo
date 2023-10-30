using MediatR;

namespace StockControl.API.Domain.Events.Moving;

/// <summary>
/// Доменное событие удаления перемещения
/// </summary>
public record MovingDeletedDomainEvent(Guid MovingId) : INotification
{
}
