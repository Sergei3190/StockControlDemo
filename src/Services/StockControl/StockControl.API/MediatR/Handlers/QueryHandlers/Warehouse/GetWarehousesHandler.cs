using MediatR;

using Service.Common.DTO;

using StockControl.API.MediatR.Queries.Warehouse;
using StockControl.API.Models.DTO.Nomenclature;
using StockControl.API.Models.DTO.Warehouse;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Warehouse;

public class GetWarehousesHandler : IRequestHandler<GetWarehousesQuery, PaginatedItemsDto<WarehouseDto>>
{
	private readonly IWarehousesService _service;

	public GetWarehousesHandler(IWarehousesService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<WarehouseDto>> Handle(GetWarehousesQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}