using MediatR;

using PersonalCabinet.API.MediatR.Commands.Document;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.Document;

public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, bool>
{
	private readonly IDocumentsService _service;

	public DeleteDocumentCommandHandler(IDocumentsService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
	{
		return await _service.DeleteAsync(request.Id).ConfigureAwait(false);
	}
}