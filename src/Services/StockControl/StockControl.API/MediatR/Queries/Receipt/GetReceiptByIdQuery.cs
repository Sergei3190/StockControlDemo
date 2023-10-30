using MediatR;

using StockControl.API.Models.DTO.Receipt;

namespace StockControl.API.MediatR.Queries.Receipt;

/// <summary>
/// Запрос на получение поступления по id
/// </summary>
/// <param name="Id"></param>
public record GetReceiptByIdQuery(Guid Id) : IRequest<ReceiptDto?>
{
}
