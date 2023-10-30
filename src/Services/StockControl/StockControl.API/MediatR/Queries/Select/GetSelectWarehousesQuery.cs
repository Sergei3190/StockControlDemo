using MediatR;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.MediatR.Queries.Select;

/// <summary>
/// Запрос на получение отфильтрованных элементов справочника склады для выпадающего списка
/// </summary>
public record GetSelectWarehousesQuery(SelectWarehouseFilterDto Filter) : IRequest<PaginatedItemsDto<NamedEntityDto>>
{
}
