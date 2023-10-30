using MediatR;

using StockControl.API.MediatR.Commands.Nomenclature;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Nomenclature;

public class DeleteNomenclatureCommandHandler : IRequestHandler<DeleteNomenclatureCommand, bool>
{
	private readonly INomenclaturesService _service;

	public DeleteNomenclatureCommandHandler(INomenclaturesService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(DeleteNomenclatureCommand request, CancellationToken cancellationToken)
	{
		// прежде чем удалять, надо сделать проверку на использование элемента справочника в уже созданных документах движения, еслти используется
		// то дать знать об этом пользователю.
		// проверка будет последовательная, чтобы не усложнять получение и выдачу информации для пользователя

		var info = await _service.GetProductFlowNumbersByItemIdAsync(request.Id).ConfigureAwait(false);

		if (info.Any())
			throw new InvalidDataException($"Удаление номенклатуры невозможно. " +
				$"Имеются проведённые документы с номерами: {string.Join(",", info.Select(r => r.number))}");

		return await _service.DeleteAsync(request.Id).ConfigureAwait(false);
	}
}