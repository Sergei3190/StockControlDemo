using MediatR;

using Note.API.MediatR.Queries;
using Note.API.Models.DTO;
using Note.API.Services.Interfaces;

using Service.Common.DTO;

namespace Note.API.MediatR.Handlers.QueryHandlers;

public class GetNotesHandler : IRequestHandler<GetNotesQuery, PaginatedItemsDto<NoteDto>>
{
	private readonly INotesService _notesService;

    public GetNotesHandler(INotesService notesService)
    {
        _notesService = notesService;
    }

    public async Task<PaginatedItemsDto<NoteDto>> Handle(GetNotesQuery request, CancellationToken cancellationToken)
	{
        return await _notesService.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}