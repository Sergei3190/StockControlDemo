using MediatR;

using Note.API.Models.DTO;

namespace Note.API.MediatR.Commands;

/// <summary>
/// Команда обновления заметки
/// </summary>
public record UpdateNoteCommand(NoteDto? Dto) : IRequest<bool>
{
}