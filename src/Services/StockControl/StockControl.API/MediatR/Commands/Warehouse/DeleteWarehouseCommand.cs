using MediatR;

namespace StockControl.API.MediatR.Commands.Warehouse;

/// <summary>
/// Команда удаления склада
/// </summary>
public record DeleteWarehouseCommand(Guid Id) : IRequest<bool>
{
}