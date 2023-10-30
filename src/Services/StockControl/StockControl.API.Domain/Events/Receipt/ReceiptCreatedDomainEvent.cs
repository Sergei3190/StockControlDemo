using MediatR;

namespace StockControl.API.Domain.Events.Receipt;

/// <summary>
/// Доменное событие создания поступления
/// </summary>
public record ReceiptCreatedDomainEvent(Guid ReceiptId) : INotification
{
}
