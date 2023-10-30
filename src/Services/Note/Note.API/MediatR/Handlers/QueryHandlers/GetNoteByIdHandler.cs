using MediatR;

using Note.API.MediatR.Queries;
using Note.API.Models.DTO;
using Note.API.Services.Interfaces;

namespace Note.API.MediatR.Handlers.QueryHandlers;

public class GetNoteByIdHandler : IRequestHandler<GetNoteByIdQuery, NoteDto?>
{
	private readonly INotesService _notesService;

	public GetNoteByIdHandler(INotesService notesService)
	{
		_notesService = notesService;
	}

	public async Task<NoteDto?> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken)
	{
		return await _notesService.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}