using MediatR;

using Service.Common.DTO;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.MediatR.Commands.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.Receipt;

public class BulkDeleteReceiptCommandHandler : IRequestHandler<BulkDeleteReceiptCommand, BulkDeleteResultDto>
{
	private readonly IReceiptsService _service;
	private readonly IPartiesService _partiesService;
	private readonly IPublisher _mediator;
	private readonly ILogger<BulkDeleteReceiptCommand> _logger;

	public BulkDeleteReceiptCommandHandler(
		IReceiptsService service, IPartiesService partiesService, IPublisher mediator, ILogger<BulkDeleteReceiptCommand> logger)
	{
		_service = service;
		_partiesService = partiesService;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task<BulkDeleteResultDto> Handle(BulkDeleteReceiptCommand request, CancellationToken cancellationToken)
	{
		var data = await _service.GetCheckingDataAsync(request.Ids).ConfigureAwait(false);

		var ids = data.Select(d => d.Id);

		_logger.LogInformation("Начинаем проверку возможности удаления поступлений с ids: {ids}..", string.Join(",", ids));

		var info = await _service.GetInvolvedQuantityAsync(data.Select(d => Tuple.Create(d.Id, d.PartyId, d.WarehouseId).ToValueTuple()).ToArray())
			.ConfigureAwait(false);

		var errorIds = info.Select(i => i.ItemId);

		string errorMessage = null!;

		if (info.Any())
		{
			var errorItems = data.Join(info, d => d.Id, i => i.ItemId, (d, i) => new
			{
				d.Number,
				i.InvolvedQuantity
			});

			errorMessage = $"Удаление поступлений: {string.Join(",", errorItems.Select(e => e.Number))} невозможно." +
				$" Имеется зарезервированная продукция (номер поступления, резерв): {string.Join(",", errorItems)}";
		}

		_logger.LogInformation("Проверка возможности удаления поступлений с ids: {ids} завершена", string.Join(",", ids));

		// отчленяем недопустимые к удалению элементы
		var successIds = request.Ids.Except(errorIds).ToArray();

		var result = await _service.BulkDeleteAsync(successIds).ConfigureAwait(false);

		if (result is null)
			throw new ArgumentNullException(nameof(result), "Получен недопустимый результат массового удаления");

		if (!string.IsNullOrEmpty(errorMessage))
			result.ErrorMessage!.Add(errorMessage);

		await _mediator.Publish(new ReceiptBulkDeletedDomainEvent(successIds)).ConfigureAwait(false);

		// не выносим в отдельную команду, тк 
		// удаление партии и документа поступления должно быть атомарной операцией
		await _partiesService.BulkDeleteAsync(data.Where(d => successIds.Contains(d.Id)).Select(d => d.PartyId).ToArray()).ConfigureAwait(false);

		return result;
	}
}