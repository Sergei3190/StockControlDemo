using MediatR;

using Note.API.Models.DTO;

namespace Note.API.MediatR.Commands;

/// <summary>
/// Команда создания заметки
/// </summary>
public record CreateNoteCommand(NoteDto? Dto) : IRequest<Guid>
{
}