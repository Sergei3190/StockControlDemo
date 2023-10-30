using MediatR;

using Service.Common.DTO;

using StockControl.API.Models.DTO.Receipt;

namespace StockControl.API.MediatR.Queries.Receipt;

/// <summary>
/// Запрос на получение отфильтрованного списка поступлений
/// </summary>
public record GetReceiptsQuery(ReceiptFilterDto Filter) : IRequest<PaginatedItemsDto<ReceiptDto>>
{
}
