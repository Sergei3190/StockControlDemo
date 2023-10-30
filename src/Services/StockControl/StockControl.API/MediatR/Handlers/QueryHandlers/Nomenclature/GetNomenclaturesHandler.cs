using MediatR;

using Service.Common.DTO;

using StockControl.API.MediatR.Queries.Nomenclature;
using StockControl.API.Models.DTO.Nomenclature;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Nomenclature;

public class GetNomenclaturesHandler : IRequestHandler<GetNomenclaturesQuery, PaginatedItemsDto<NomenclatureDto>>
{
	private readonly INomenclaturesService _service;

	public GetNomenclaturesHandler(INomenclaturesService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<NomenclatureDto>> Handle(GetNomenclaturesQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}