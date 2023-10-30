using MediatR;

namespace StockControl.API.Domain.Events.WriteOff;

/// <summary>
/// Доменное событие удаления списания
/// </summary>
public record WriteOffDeletedDomainEvent(Guid WriteOffId) : INotification
{
}
