using MediatR;

using StockControl.API.MediatR.Queries.Nomenclature;
using StockControl.API.Models.DTO.Nomenclature;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Nomenclature;

public class GetNomenclatureByIdHandler : IRequestHandler<GetNomenclatureByIdQuery, NomenclatureDto?>
{
	private readonly INomenclaturesService _service;

	public GetNomenclatureByIdHandler(INomenclaturesService service)
	{
		_service = service;
	}

	public async Task<NomenclatureDto?> Handle(GetNomenclatureByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}