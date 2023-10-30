using MediatR;

namespace StockControl.API.Domain.Events.Receipt;

/// <summary>
/// Доменное событие обновления поступления
/// </summary>
public record ReceiptUpdatedDomainEvent(Guid ReceiptId) : INotification
{
}
