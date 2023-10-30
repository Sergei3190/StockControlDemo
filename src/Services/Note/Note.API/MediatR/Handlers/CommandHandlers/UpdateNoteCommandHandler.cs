using MediatR;

using Note.API.MediatR.Commands;
using Note.API.Services.Interfaces;

namespace Note.API.MediatR.Handlers.CommandHandlers;

public class UpdateNoteCommandHandler : IRequestHandler<UpdateNoteCommand, bool>
{
	private readonly INotesService _notesService;

	public UpdateNoteCommandHandler(INotesService notesService)
	{
		_notesService = notesService;
	}

	public async Task<bool> Handle(UpdateNoteCommand request, CancellationToken cancellationToken)
	{
		return await _notesService.UpdateAsync(request.Dto).ConfigureAwait(false);	
	}
}