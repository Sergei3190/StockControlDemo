using MediatR;

using PersonalCabinet.API.MediatR.Commands.Document;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.Document;

public class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, Guid>
{
	private readonly IDocumentsService _service;

	public CreateDocumentCommandHandler(IDocumentsService service)
	{
		_service = service;
	}

	public async Task<Guid> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
	{
		return await _service.CreateAsync(request.Dto).ConfigureAwait(false);
	}
}