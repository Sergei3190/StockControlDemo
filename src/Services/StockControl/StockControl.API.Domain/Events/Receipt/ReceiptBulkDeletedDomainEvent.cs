using MediatR;

namespace StockControl.API.Domain.Events.Receipt;

/// <summary>
/// Доменное событие массового удаления поступления
/// </summary>
public record ReceiptBulkDeletedDomainEvent(Guid[] ReceiptIds) : INotification
{
}
