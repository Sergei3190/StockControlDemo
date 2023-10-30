using MediatR;

using StockControl.API.MediatR.Queries.WriteOff;
using StockControl.API.Models.DTO.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.WriteOff;

public class GetWriteOffByIdHandler : IRequestHandler<GetWriteOffByIdQuery, WriteOffDto?>
{
	private readonly IWriteOffsService _service;

	public GetWriteOffByIdHandler(IWriteOffsService service)
	{
		_service = service;
	}

	public async Task<WriteOffDto?> Handle(GetWriteOffByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}