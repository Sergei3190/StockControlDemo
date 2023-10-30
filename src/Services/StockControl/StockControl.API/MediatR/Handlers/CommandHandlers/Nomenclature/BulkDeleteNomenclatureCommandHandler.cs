using MediatR;

using Service.Common.DTO;

using StockControl.API.MediatR.Commands.Nomenclature;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Nomenclature;

public class BulkDeleteNomenclatureCommandHandler : IRequestHandler<BulkDeleteNomenclatureCommand, BulkDeleteResultDto>
{
	private readonly INomenclaturesService _service;

	public BulkDeleteNomenclatureCommandHandler(INomenclaturesService service)
	{
		_service = service;
	}

	public async Task<BulkDeleteResultDto> Handle(BulkDeleteNomenclatureCommand request, CancellationToken cancellationToken)
	{
		// прежде чем удалять, надо сделать проверку на использование элемента справочника в уже созданных документах движения, еслти используется
		// то дать знать об этом пользователю.
		// проверка будет последовательная, чтобы не усложнять получение и выдачу информации для пользователя

		var info = await _service.GetProductFlowNumbersByItemIdAsync(request.Ids).ConfigureAwait(false);

		var errorIds = info.Select(i => i.itemId);

		// отчленяем недопустимые к удалению элементы
		var successIds = request.Ids.Except(errorIds).ToArray();

		var result = await _service.BulkDeleteAsync(successIds).ConfigureAwait(false);

		if (result is null)
			throw new ArgumentNullException(nameof(result), "Получен недопустимый результат массового удаления");

		if (info.Any())
		{
			result.ErrorMessage!.Add($"Удаление номенклатуры: {string.Join(",", info.Select(i => i.name))} невозможно." +
				$" Имеются проведённые документы с номерами: {string.Join(",", info.Select(i => i.number))}");
		}

		return result;
	}
}