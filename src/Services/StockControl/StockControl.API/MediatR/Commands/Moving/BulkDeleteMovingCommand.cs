using MediatR;

using Service.Common.DTO;

namespace StockControl.API.MediatR.Commands.Moving;

/// <summary>
/// Команда массового удаления перемещения
/// </summary>
public record BulkDeleteMovingCommand(params Guid[] Ids) : IRequest<BulkDeleteResultDto>
{
}