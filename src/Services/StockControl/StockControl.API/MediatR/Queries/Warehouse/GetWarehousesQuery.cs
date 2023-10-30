using MediatR;

using Service.Common.DTO;

using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.MediatR.Queries.Warehouse;

/// <summary>
/// Запрос на получение отфильтрованного элемента справочника склады
/// </summary>
public record GetWarehousesQuery(WarehouseFilterDto Filter) : IRequest<PaginatedItemsDto<WarehouseDto>>
{
}
