using MediatR;

using PersonalCabinet.API.MediatR.Commands.Document;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.Document;

public class UpdateDocumentCommandHandler : IRequestHandler<UpdateDocumentCommand, bool>
{
	private readonly IDocumentsService _service;

	public UpdateDocumentCommandHandler(IDocumentsService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(UpdateDocumentCommand request, CancellationToken cancellationToken)
	{
		return await _service.UpdateAsync(request.Dto).ConfigureAwait(false);
	}
}