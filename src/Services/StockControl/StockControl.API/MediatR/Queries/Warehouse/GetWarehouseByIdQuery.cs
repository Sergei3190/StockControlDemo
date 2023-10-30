using MediatR;

using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.MediatR.Queries.Warehouse;

/// <summary>
/// Запрос на получение элемента справочника склады по id
/// </summary>
public record GetWarehouseByIdQuery(Guid Id) : IRequest<WarehouseDto?>
{
}
