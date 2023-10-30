using MediatR;

using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.MediatR.Commands.Warehouse;

/// <summary>
/// Команда обновления склада
/// </summary>
public record UpdateWarehouseCommand(WarehouseDto? Dto) : IRequest<bool>
{
}