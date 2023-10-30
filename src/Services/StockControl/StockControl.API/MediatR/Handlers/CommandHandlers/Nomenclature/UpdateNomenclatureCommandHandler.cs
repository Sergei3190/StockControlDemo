using MediatR;

using StockControl.API.MediatR.Commands.Nomenclature;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Nomenclature;

public class UpdateNomenclatureCommandHandler : IRequestHandler<UpdateNomenclatureCommand, bool>
{
	private readonly INomenclaturesService _service;

	public UpdateNomenclatureCommandHandler(INomenclaturesService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(UpdateNomenclatureCommand request, CancellationToken cancellationToken)
	{
		return await _service.UpdateAsync(request.Dto).ConfigureAwait(false);
	}
}