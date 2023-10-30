using MediatR;

using Note.API.Models.DTO;

using Service.Common.DTO;

namespace Note.API.MediatR.Queries;

/// <summary>
/// Запрос на получение отфильтрованного списка заметок 
/// </summary>
public record GetNotesQuery(NoteFilterDto Filter) : IRequest<PaginatedItemsDto<NoteDto>>
{
}
