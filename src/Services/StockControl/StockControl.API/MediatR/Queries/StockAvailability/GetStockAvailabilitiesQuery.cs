using MediatR;

using Service.Common.DTO;

using StockControl.API.Models.DTO.StockAvailability;

namespace StockControl.API.MediatR.Queries.StockAvailability;

/// <summary>
/// Запрос на получение отфильтрованного списка остатков
/// </summary>
public record GetStockAvailabilitiesQuery(StockAvailabilityFilterDto Filter) : IRequest<PaginatedItemsDto<StockAvailabilityDto>>
{
}
