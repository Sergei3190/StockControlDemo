using MediatR;

namespace StockControl.API.Domain.Events.WriteOff;

/// <summary>
/// Доменное событие создания списания
/// </summary>
public record WriteOffCreatedDomainEvent(Guid WriteOffId) : INotification
{
}
