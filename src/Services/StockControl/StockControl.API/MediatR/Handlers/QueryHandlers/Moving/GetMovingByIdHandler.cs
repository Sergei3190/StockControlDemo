using MediatR;

using StockControl.API.MediatR.Queries.Moving;
using StockControl.API.Models.DTO.Moving;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Moving;

public class GetMovingByIdHandler : IRequestHandler<GetMovingByIdQuery, MovingDto?>
{
	private readonly IMovingsService _service;

	public GetMovingByIdHandler(IMovingsService service)
	{
		_service = service;
	}

	public async Task<MovingDto?> Handle(GetMovingByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}