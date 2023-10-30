using MediatR;

using StockControl.API.Models.DTO.Moving;

namespace StockControl.API.MediatR.Commands.Moving;

/// <summary>
/// Команда создания перемещения
/// </summary>
public record CreateMovingCommand(MovingDto? Dto) : IRequest<Guid>
{
}