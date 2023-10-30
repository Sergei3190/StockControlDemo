using MediatR;

namespace StockControl.API.Domain.Events.WriteOff;

/// <summary>
/// Доменное событие обновления списания
/// </summary>
public record WriteOffUpdatedDomainEvent(Guid WriteOffId) : INotification
{
}
