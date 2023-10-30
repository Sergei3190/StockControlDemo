using MediatR;

using Note.API.Models.DTO;

namespace Note.API.MediatR.Queries;

/// <summary>
/// Запрос на получение заметки по id
/// </summary>
/// <param name="Id"></param>
public record GetNoteByIdQuery(Guid Id) : IRequest<NoteDto?>
{
}
