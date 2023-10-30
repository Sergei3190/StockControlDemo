using MediatR;
using StockControl.API.Models.DTO.WriteOff;

namespace StockControl.API.MediatR.Commands.WriteOff;

/// <summary>
/// Команда создания списания
/// </summary>
public record CreateWriteOffCommand(WriteOffDto? Dto) : IRequest<Guid>
{
}