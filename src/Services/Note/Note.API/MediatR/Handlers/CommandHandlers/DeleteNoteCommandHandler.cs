using MediatR;

using Note.API.MediatR.Commands;
using Note.API.Services.Interfaces;

namespace Note.API.MediatR.Handlers.CommandHandlers;

public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand, bool>
{
	private readonly INotesService _notesService;

	public DeleteNoteCommandHandler(INotesService notesService)
	{
		_notesService = notesService;
	}

	public async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
	{
		return await _notesService.DeleteAsync(request.Id).ConfigureAwait(false);	
	}
}