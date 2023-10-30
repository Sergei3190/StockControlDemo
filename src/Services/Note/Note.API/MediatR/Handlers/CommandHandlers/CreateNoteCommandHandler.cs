using MediatR;

using Note.API.MediatR.Commands;
using Note.API.Services.Interfaces;

namespace Note.API.MediatR.Handlers.CommandHandlers;

public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Guid>
{
	private readonly INotesService _notesService;

	public CreateNoteCommandHandler(INotesService notesService)
	{
		_notesService = notesService;
	}

	public async Task<Guid> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
	{
		return await _notesService.CreateAsync(request.Dto).ConfigureAwait(false);	
	}
}