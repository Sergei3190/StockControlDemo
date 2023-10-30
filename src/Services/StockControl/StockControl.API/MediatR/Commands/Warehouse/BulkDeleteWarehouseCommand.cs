using MediatR;

using Service.Common.DTO;

namespace StockControl.API.MediatR.Commands.Warehouse;

/// <summary>
/// Команда массового удаления складов
/// </summary>
public record BulkDeleteWarehouseCommand(params Guid[] Ids) : IRequest<BulkDeleteResultDto>
{
}