using MediatR;

using PersonalCabinet.API.MediatR.Queries.Document;
using PersonalCabinet.API.Models.DTO.Document;
using PersonalCabinet.API.Services.Interfaces;

using Service.Common.DTO;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.Document;

public class GetDocumentsHandler : IRequestHandler<GetDocumentsQuery, PaginatedItemsDto<DocumentDto>>
{
	private readonly IDocumentsService _service;

	public GetDocumentsHandler(IDocumentsService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<DocumentDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}