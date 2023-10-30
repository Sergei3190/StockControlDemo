using MediatR;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.MediatR.Commands.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.Receipt;

public class DeleteReceiptCommandHandler : IRequestHandler<DeleteReceiptCommand, bool>
{
	private readonly IReceiptsService _service;
	private readonly IPartiesService _partiesService;
	private readonly IPublisher _mediator;
	private readonly ILogger<DeleteReceiptCommand> _logger;

	public DeleteReceiptCommandHandler(
		IReceiptsService service, IPartiesService partiesService, IPublisher mediator, ILogger<DeleteReceiptCommand> logger)
	{
		_service = service;
		_partiesService = partiesService;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task<bool> Handle(DeleteReceiptCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request, nameof(request));

		var data = await _service.GetCheckingDataAsync(request.Id).ConfigureAwait(false);

		var singlData = data.First();

		_logger.LogInformation("Начинаем проверку возможности удаления поступления с id = {id}..", singlData.Id);

		var info = await _service.GetInvolvedQuantityAsync((singlData.Id, singlData.PartyId, singlData.WarehouseId))
			.ConfigureAwait(false);

		if (info.Count() == 1)
		{
			var error = string.Format("Удаление поступления невозможно. " +
				"Имеется зарезервированная продукция в кол-ве {0}.", info.First().InvolvedQuantity);
			throw new ArgumentException(error, nameof(request.Id));
		}

		_logger.LogInformation("Проверка возможности удаления поступления с id = {id} завершена успешно", singlData.Id);

		var result = await _service.DeleteAsync(request.Id).ConfigureAwait(false);

		if (result)
			await _mediator.Publish(new ReceiptDeletedDomainEvent(request.Id)).ConfigureAwait(false);

		// не выносим в отдельную команду, тк 
		// удаление партии и документа поступления должно быть атомарной операцией
		await _partiesService.DeleteAsync(singlData.PartyId).ConfigureAwait(false);

		return result;
	}
}