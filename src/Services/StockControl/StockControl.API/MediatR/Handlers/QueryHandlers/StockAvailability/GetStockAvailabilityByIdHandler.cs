using MediatR;

using StockControl.API.MediatR.Queries.StockAvailability;
using StockControl.API.Models.DTO.StockAvailability;
using StockControl.API.Services.Interfaces;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.StockAvailability;

public class GetStockAvailabilityByIdHandler : IRequestHandler<GetStockAvailabilityByIdQuery, StockAvailabilityDto?>
{
	private readonly IStockAvailabilitiesService _service;

	public GetStockAvailabilityByIdHandler(IStockAvailabilitiesService service)
	{
		_service = service;
	}

	public async Task<StockAvailabilityDto?> Handle(GetStockAvailabilityByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}