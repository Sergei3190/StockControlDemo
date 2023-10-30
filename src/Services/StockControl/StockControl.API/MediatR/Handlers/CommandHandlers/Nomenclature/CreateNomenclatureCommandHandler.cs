using MediatR;

using StockControl.API.MediatR.Commands.Nomenclature;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Nomenclature;

public class CreateNomenclatureCommandHandler : IRequestHandler<CreateNomenclatureCommand, Guid>
{
	private readonly INomenclaturesService _service;

	public CreateNomenclatureCommandHandler(INomenclaturesService service)
	{
		_service = service;
	}

	public async Task<Guid> Handle(CreateNomenclatureCommand request, CancellationToken cancellationToken)
	{
		return await _service.CreateAsync(request.Dto).ConfigureAwait(false);
	}
}