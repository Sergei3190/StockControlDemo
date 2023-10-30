using MediatR;

using Service.Common.DTO;

using StockControl.API.MediatR.Queries.Organization;
using StockControl.API.Models.DTO.Organization;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Organization;

public class GetOrganizationsHandler : IRequestHandler<GetOrganizationsQuery, PaginatedItemsDto<OrganizationDto>>
{
	private readonly IOrganizationsService _service;

	public GetOrganizationsHandler(IOrganizationsService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<OrganizationDto>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}