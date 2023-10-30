using MediatR;

using StockControl.API.MediatR.Commands.Warehouse;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Warehouse;

public class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand, bool>
{
	private readonly IWarehousesService _service;

	public DeleteWarehouseCommandHandler(IWarehousesService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
	{
		// прежде чем удалять, надо сделать проверку на использование элемента справочника в уже созданных документах движения, еслти используется
		// то дать знать об этом пользователю.
		// проверка будет последовательная, чтобы не усложнять получение и выдачу информации для пользователя

		var info = await _service.GetProductFlowNumbersByItemIdAsync(request.Id).ConfigureAwait(false);

		if (info.Any())
			throw new InvalidDataException($"Удаление склада невозможно." +
				$" Имеются проведённые документы с номерами: {string.Join(",", info.Select(i => i.number))}");

		return await _service.DeleteAsync(request.Id).ConfigureAwait(false);
	}
}