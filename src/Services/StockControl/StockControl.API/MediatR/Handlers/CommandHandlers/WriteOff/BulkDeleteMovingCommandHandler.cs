using MediatR;

using Service.Common.DTO;

using StockControl.API.Domain.Events.WriteOff;
using StockControl.API.MediatR.Commands.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.WriteOff;

public class BulkDeleteWriteOffCommandHandler : IRequestHandler<BulkDeleteWriteOffCommand, BulkDeleteResultDto>
{
	private readonly IWriteOffsService _service;
	private readonly IPublisher _mediator;
	private readonly ILogger<BulkDeleteWriteOffCommand> _logger;

	public BulkDeleteWriteOffCommandHandler(
		IWriteOffsService service, IPublisher mediator, ILogger<BulkDeleteWriteOffCommand> logger)
	{
		_service = service;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task<BulkDeleteResultDto> Handle(BulkDeleteWriteOffCommand request, CancellationToken cancellationToken)
	{
		var data = await _service.GetCheckingDataAsync(request.Ids).ConfigureAwait(false);

		var ids = data.Select(d => d.Id);

		_logger.LogInformation("Начинаем проверку возможности удаления списаний с ids: {ids}..", string.Join(",", ids));

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

			errorMessage = $"Удаление списаний: {string.Join(",", errorItems.Select(e => e.Number))} невозможно." +
				$" Имеется зарезервированная продукция (номер поступления, резерв): {string.Join(",", errorItems)}";
		}

		_logger.LogInformation("Проверка возможности удаления списаний с ids: {ids} завершена", string.Join(",", ids));

		// отчленяем недопустимые к удалению элементы
		var successIds = request.Ids.Except(errorIds).ToArray();

		var result = await _service.BulkDeleteAsync(successIds).ConfigureAwait(false);

		if (result is null)
			throw new ArgumentNullException(nameof(result), "Получен недопустимый результат массового удаления");

		if (!string.IsNullOrEmpty(errorMessage))
			result.ErrorMessage!.Add(errorMessage);

		await _mediator.Publish(new WriteOffBulkDeletedDomainEvent(successIds)).ConfigureAwait(false);

		return result;
	}
}