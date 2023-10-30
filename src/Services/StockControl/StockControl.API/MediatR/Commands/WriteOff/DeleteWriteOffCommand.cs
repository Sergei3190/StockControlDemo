using MediatR;

namespace StockControl.API.MediatR.Commands.WriteOff;

/// <summary>
/// Команда удаления списания
/// </summary>
public record DeleteWriteOffCommand(Guid Id) : IRequest<bool>
{
}