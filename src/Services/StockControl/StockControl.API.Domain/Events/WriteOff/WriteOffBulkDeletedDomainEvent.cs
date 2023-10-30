using MediatR;

namespace StockControl.API.Domain.Events.WriteOff;

/// <summary>
/// Доменное событие массового удаления списания
/// </summary>
public record WriteOffBulkDeletedDomainEvent(Guid[] WriteOffIds) : INotification
{
}
