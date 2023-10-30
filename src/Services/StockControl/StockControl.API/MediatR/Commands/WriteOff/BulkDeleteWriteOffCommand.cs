using MediatR;

using Service.Common.DTO;

namespace StockControl.API.MediatR.Commands.WriteOff;

/// <summary>
/// Команда массового удаления списаний
/// </summary>
public record BulkDeleteWriteOffCommand(params Guid[] Ids) : IRequest<BulkDeleteResultDto>
{
}