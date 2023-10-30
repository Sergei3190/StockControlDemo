using MediatR;

using StockControl.API.MediatR.Queries.Warehouse;
using StockControl.API.Models.DTO.Warehouse;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Warehouse;

public class GetWarehouseByIdHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseDto?>
{
	private readonly IWarehousesService _service;

	public GetWarehouseByIdHandler(IWarehousesService service)
	{
		_service = service;
	}

	public async Task<WarehouseDto?> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}