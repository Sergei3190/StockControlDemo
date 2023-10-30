using MediatR;

using Note.API.Models.DTO;

namespace Note.API.MediatR.Commands;

/// <summary>
/// Команда обновления сортировки списка заметок
/// </summary>
public record UpdateSortCommand(NoteDto[] DtoArray) : IRequest<bool>
{
}