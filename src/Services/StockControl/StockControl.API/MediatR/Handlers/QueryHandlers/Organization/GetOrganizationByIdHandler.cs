using MediatR;

using StockControl.API.MediatR.Queries.Organization;
using StockControl.API.Models.DTO.Organization;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Organization;

public class GetOrganizationByIdHandler : IRequestHandler<GetOrganizationByIdQuery, OrganizationDto?>
{
	private readonly IOrganizationsService _service;

	public GetOrganizationByIdHandler(IOrganizationsService service)
	{
		_service = service;
	}

	public async Task<OrganizationDto?> Handle(GetOrganizationByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}