using MediatR;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.MediatR.Commands.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.WriteOff;

public class DeleteWriteOffCommandHandler : IRequestHandler<DeleteWriteOffCommand, bool>
{
	private readonly IWriteOffsService _service;
	private readonly ILogger<DeleteWriteOffCommandHandler> _logger;
	private readonly IPublisher _mediator;

	public DeleteWriteOffCommandHandler(
		IWriteOffsService service, ILogger<DeleteWriteOffCommandHandler> logger, IPublisher mediator)
	{
		_service = service;
		_logger = logger;
		_mediator = mediator;
	}

	public async Task<bool> Handle(DeleteWriteOffCommand request, CancellationToken cancellationToken)
	{
		var data = await _service.GetCheckingDataAsync(request.Id).ConfigureAwait(false);

		var singlData = data.First();

		_logger.LogInformation("Начинаем проверку возможности удаления списания с id = {id}..", singlData.Id);

		var info = await _service.GetInvolvedQuantityAsync((singlData.Id, singlData.PartyId, singlData.WarehouseId))
			.ConfigureAwait(false);

		if (info.Count() == 1)
		{
			var error = string.Format("Удаление списания невозможно. " +
				"Имеется зарезервированная продукция в кол-ве {0}.", info.First().InvolvedQuantity);
			throw new ArgumentException(error, nameof(request.Id));
		}

		_logger.LogInformation("Проверка возможности удаления списания с id = {id} завершена успешно", singlData.Id);

		var result = await _service.DeleteAsync(request.Id).ConfigureAwait(false);

		if (result)
			await _mediator.Publish(new ReceiptDeletedDomainEvent(request.Id)).ConfigureAwait(false);

		return result;
	}
}