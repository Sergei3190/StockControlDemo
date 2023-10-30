using MediatR;

using PersonalCabinet.API.MediatR.Queries.Document;
using PersonalCabinet.API.Models.DTO.Document;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.Document;

public class GetDocumentByIdHandler : IRequestHandler<GetDocumentByIdQuery, DocumentDto?>
{
	private readonly IDocumentsService _service;

	public GetDocumentByIdHandler(IDocumentsService service)
	{
		_service = service;
	}

	public async Task<DocumentDto?> Handle(GetDocumentByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}