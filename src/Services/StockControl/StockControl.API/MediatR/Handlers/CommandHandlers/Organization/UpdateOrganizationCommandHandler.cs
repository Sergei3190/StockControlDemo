using MediatR;

using StockControl.API.MediatR.Commands.Organization;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Organization;

public class UpdateOrganizationCommandHandler : IRequestHandler<UpdateOrganizationCommand, bool>
{
	private readonly IOrganizationsService _service;

	public UpdateOrganizationCommandHandler(IOrganizationsService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(UpdateOrganizationCommand request, CancellationToken cancellationToken)
	{
		return await _service.UpdateAsync(request.Dto).ConfigureAwait(false);
	}
}