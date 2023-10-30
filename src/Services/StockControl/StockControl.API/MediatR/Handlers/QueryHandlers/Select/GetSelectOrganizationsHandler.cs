using MediatR;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Select;

public class GetSelectOrganizationsHandler : IRequestHandler<GetSelectOrganizationsQuery, PaginatedItemsDto<NamedEntityDto>>
{
	private readonly ISelectOrganizationsService _service;

	public GetSelectOrganizationsHandler(ISelectOrganizationsService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> Handle(GetSelectOrganizationsQuery request, CancellationToken cancellationToken)
	{
		return await _service.SelectAsync(request.Filter).ConfigureAwait(false);
	}
}