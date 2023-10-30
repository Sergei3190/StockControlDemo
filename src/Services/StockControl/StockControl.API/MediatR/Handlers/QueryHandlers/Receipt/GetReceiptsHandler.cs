using MediatR;

using Service.Common.DTO;

using StockControl.API.MediatR.Queries.Receipt;
using StockControl.API.Models.DTO.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Receipt;

public class GetReceiptsHandler : IRequestHandler<GetReceiptsQuery, PaginatedItemsDto<ReceiptDto>>
{
	private readonly IReceiptsService _service;

	public GetReceiptsHandler(IReceiptsService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<ReceiptDto>> Handle(GetReceiptsQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}