using MediatR;

using StockControl.API.MediatR.Commands.Warehouse;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Warehouse;

public class UpdateWarehouseCommandHandler : IRequestHandler<UpdateWarehouseCommand, bool>
{
	private readonly IWarehousesService _service;

	public UpdateWarehouseCommandHandler(IWarehousesService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
	{
		return await _service.UpdateAsync(request.Dto).ConfigureAwait(false);
	}
}