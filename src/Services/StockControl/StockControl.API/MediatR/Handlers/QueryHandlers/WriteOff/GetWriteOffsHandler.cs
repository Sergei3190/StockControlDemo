using MediatR;

using Service.Common.DTO;

using StockControl.API.MediatR.Queries.WriteOff;
using StockControl.API.Models.DTO.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.WriteOff;

public class GetWriteOffsHandler : IRequestHandler<GetWriteOffsQuery, PaginatedItemsDto<WriteOffDto>>
{
	private readonly IWriteOffsService _service;

	public GetWriteOffsHandler(IWriteOffsService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<WriteOffDto>> Handle(GetWriteOffsQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}