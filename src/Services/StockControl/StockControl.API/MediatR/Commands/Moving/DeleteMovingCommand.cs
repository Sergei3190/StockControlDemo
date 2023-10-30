using MediatR;

namespace StockControl.API.MediatR.Commands.Moving;

/// <summary>
/// Команда удаления перемещения
/// </summary>
public record DeleteMovingCommand(Guid Id) : IRequest<bool>
{
}