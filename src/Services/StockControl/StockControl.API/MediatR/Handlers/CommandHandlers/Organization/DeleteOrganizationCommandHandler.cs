using MediatR;

using StockControl.API.MediatR.Commands.Organization;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.MediatR.CommandHandlers.Organization;

public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand, bool>
{
	private readonly IOrganizationsService _service;

	public DeleteOrganizationCommandHandler(IOrganizationsService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
	{
		// прежде чем удалять, надо сделать проверку на использование элемента справочника в уже созданных документах движения, еслти используется
		// то дать знать об этом пользователю.
		// проверка будет последовательная, чтобы не усложнять получение и выдачу информации для пользователя

		var info = await _service.GetProductFlowNumbersByItemIdAsync(request.Id).ConfigureAwait(false);

		if (info.Any())
			throw new InvalidDataException($"Удаление организации невозможно. " +
				$"Имеются проведённые документы с номерами: {string.Join(",", info.Select(i => i.number))}");

		return await _service.DeleteAsync(request.Id).ConfigureAwait(false);
	}
}