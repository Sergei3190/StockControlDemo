using MediatR;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.DTO.ProductFlowType;

namespace StockControl.API.MediatR.Queries.Select;

/// <summary>
/// Запрос на получение отфильтрованных типов движения товара
/// </summary>
public record GetProductFlowTypesQuery(ProductFlowTypeFilterDto Filter) : IRequest<PaginatedItemsDto<NamedEntityDto>>
{
}
