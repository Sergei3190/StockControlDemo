using MediatR;

using Note.API.MediatR.Commands;
using Note.API.Services.Interfaces;

namespace Note.API.MediatR.Handlers.CommandHandlers;

public class UpdateSortCommandHandler : IRequestHandler<UpdateSortCommand, bool>
{
	private readonly INotesService _notesService;

	public UpdateSortCommandHandler(INotesService notesService)
	{
		_notesService = notesService;
	}

	public async Task<bool> Handle(UpdateSortCommand request, CancellationToken cancellationToken)
	{
		return await _notesService.UpdateSortAsync(request.DtoArray).ConfigureAwait(false);
	}
}