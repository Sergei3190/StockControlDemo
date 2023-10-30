using MediatR;

using StockControl.API.MediatR.Commands.Organization;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Organization;

public class CreateOrganizationCommandHandler : IRequestHandler<CreateOrganizationCommand, Guid>
{
	private readonly IOrganizationsService _service;

	public CreateOrganizationCommandHandler(IOrganizationsService service)
	{
		_service = service;
	}

	public async Task<Guid> Handle(CreateOrganizationCommand request, CancellationToken cancellationToken)
	{
		return await _service.CreateAsync(request.Dto).ConfigureAwait(false);
	}
}