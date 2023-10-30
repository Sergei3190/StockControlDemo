using MediatR;

using Service.Common.DTO;

using StockControl.API.MediatR.Queries.Moving;
using StockControl.API.Models.DTO.Moving;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Moving;

public class GetMovingsHandler : IRequestHandler<GetMovingsQuery, PaginatedItemsDto<MovingDto>>
{
	private readonly IMovingsService _service;

	public GetMovingsHandler(IMovingsService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<MovingDto>> Handle(GetMovingsQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}