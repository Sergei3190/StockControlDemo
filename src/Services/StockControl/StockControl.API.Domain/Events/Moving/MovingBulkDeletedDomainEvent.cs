using MediatR;

namespace StockControl.API.Domain.Events.Moving;

/// <summary>
/// Доменное событие массового удаления перемещения
/// </summary>
public record MovingBulkDeletedDomainEvent(Guid[] MovingIds) : INotification
{
}
