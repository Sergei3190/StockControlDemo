using MediatR;

using StockControl.API.Models.DTO.Moving;

namespace StockControl.API.MediatR.Commands.Moving;

/// <summary>
/// Команда обновления перемещения
/// </summary>
public record UpdateMovingCommand(MovingDto? Dto) : IRequest<bool>
{
}