using MediatR;

using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.MediatR.Commands.Warehouse;

/// <summary>
/// Команда создания склада
/// </summary>
public record CreateWarehouseCommand(WarehouseDto? Dto) : IRequest<Guid>
{
}