using MediatR;

namespace Note.API.MediatR.Commands;

/// <summary>
/// Команда удаления заметки
/// </summary>
public record DeleteNoteCommand(Guid Id) : IRequest<bool>
{
}