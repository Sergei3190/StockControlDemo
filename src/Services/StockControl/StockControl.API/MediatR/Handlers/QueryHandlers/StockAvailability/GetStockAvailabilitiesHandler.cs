using MediatR;

using Service.Common.DTO;

using StockControl.API.MediatR.Queries.StockAvailability;
using StockControl.API.Models.DTO.StockAvailability;
using StockControl.API.Services.Interfaces;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.StockAvailability;

public class GetStockAvailabilitiesHandler : IRequestHandler<GetStockAvailabilitiesQuery, PaginatedItemsDto<StockAvailabilityDto>>
{
	private readonly IStockAvailabilitiesService _service;

	public GetStockAvailabilitiesHandler(IStockAvailabilitiesService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<StockAvailabilityDto>> Handle(GetStockAvailabilitiesQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}