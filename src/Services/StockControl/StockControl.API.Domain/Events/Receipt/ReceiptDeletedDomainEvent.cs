using MediatR;

namespace StockControl.API.Domain.Events.Receipt;

/// <summary>
/// Доменное событие удаления поступления
/// </summary>
public record ReceiptDeletedDomainEvent(Guid ReceiptId) : INotification
{
}
