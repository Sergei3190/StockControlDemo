using MediatR;
using StockControl.API.Models.DTO.WriteOff;

namespace StockControl.API.MediatR.Commands.WriteOff;

/// <summary>
/// Команда обновления списания
/// </summary>
public record UpdateWriteOffCommand(WriteOffDto? Dto) : IRequest<bool>
{
}