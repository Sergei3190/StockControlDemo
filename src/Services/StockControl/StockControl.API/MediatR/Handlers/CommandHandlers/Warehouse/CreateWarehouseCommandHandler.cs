using MediatR;

using StockControl.API.MediatR.Commands.Warehouse;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Warehouse;

public class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, Guid>
{
	private readonly IWarehousesService _service;

	public CreateWarehouseCommandHandler(IWarehousesService service)
	{
		_service = service;
	}

	public async Task<Guid> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
	{
		return await _service.CreateAsync(request.Dto).ConfigureAwait(false);
	}
}