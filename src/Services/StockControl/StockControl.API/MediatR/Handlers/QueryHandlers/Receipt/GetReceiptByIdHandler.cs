using MediatR;

using StockControl.API.MediatR.Queries.Receipt;
using StockControl.API.Models.DTO.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Receipt;

public class GetReceiptByIdHandler : IRequestHandler<GetReceiptByIdQuery, ReceiptDto?>
{
	private readonly IReceiptsService _service;

	public GetReceiptByIdHandler(IReceiptsService service)
	{
		_service = service;
	}

	public async Task<ReceiptDto?> Handle(GetReceiptByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}