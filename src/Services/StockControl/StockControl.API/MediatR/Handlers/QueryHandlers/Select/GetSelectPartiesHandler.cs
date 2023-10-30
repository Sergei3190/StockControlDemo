using MediatR;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Select;

public class GetSelectPartiesHandler : IRequestHandler<GetSelectPartiesQuery, PaginatedItemsDto<NamedEntityDto>>
{
	private readonly ISelectPartiesService _service;

	public GetSelectPartiesHandler(ISelectPartiesService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> Handle(GetSelectPartiesQuery request, CancellationToken cancellationToken)
	{
		return await _service.SelectAsync(request.Filter).ConfigureAwait(false);
	}
}