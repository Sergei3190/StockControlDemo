using MediatR;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Select;

public class GetSelectWarehousesHandler : IRequestHandler<GetSelectWarehousesQuery, PaginatedItemsDto<NamedEntityDto>>
{
	private readonly ISelectWarehousesService _service;

	public GetSelectWarehousesHandler(ISelectWarehousesService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> Handle(GetSelectWarehousesQuery request, CancellationToken cancellationToken)
	{
		return await _service.SelectAsync(request.Filter).ConfigureAwait(false);
	}
}