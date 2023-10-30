using MediatR;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.MediatR.Commands.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.Receipt;

public class UpdateReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand, bool>
{
	private readonly IReceiptsService _service;
	private readonly IPartiesService _partiesService;
	private readonly IPublisher _mediator;
	private readonly ILogger<UpdateReceiptCommand> _logger;

	public UpdateReceiptCommandHandler(IReceiptsService service, IPartiesService partiesService, IPublisher mediator, ILogger<UpdateReceiptCommand> logger)
	{
		_service = service;
		_partiesService = partiesService;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task<bool> Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request, nameof(request));

		var dto = request.Dto;

		if (dto is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(UpdateReceiptCommand));
			throw new ArgumentNullException(error, nameof(dto));
		}

		// не выносим в отдельную команду, тк 
		// обновление партии и документа поступления должно быть атомарной операцией
		await _partiesService.UpdateAsync(dto.Party).ConfigureAwait(false);

		_logger.LogInformation("Начинаем проверку возможности изменения кол-ва поступления с id = {id}..", dto.Id);

		// прежде чем изменить остаток, мы должны проверить задействована ли партия остатка в других движениях товара, те 
		// были ли сделаны перемещения или списания по этой партии, где складом отправителя является склад хранения этой партии,
		// если были сделаны, то нельзя уменьшить остаток на недопустимое для отрицательных остатков кол-во
		var newQuantity = dto.Quantity;

		// смотрим только у не удаленных движений (это могут быть только списания или перемещения, тк при создании поступления
		// даже с теми же параметрами все равно будет создана новая партия),
		// при удалении списания или перемещения остатки плюсуются на складе отправителе НО не минусуются на складе получателя, как в самих списаниях и перемещениях
		// кол-во не меняется - для истории!!!
		var info = await _service.GetInvolvedQuantityAsync((dto.Id, dto.Party!.Id, dto.Warehouse.Id))
			.ConfigureAwait(false);

		if (info.Any())
		{
			var involvedQuantity = info.First();

			var differenceQuantity = newQuantity - involvedQuantity.InvolvedQuantity;

			if (differenceQuantity < 0)
			{
				var error = string.Format("Указано недопустимое кол-во ({0}) номенклатуры. Минимальный остаток должен быть равен {1}." +
					" Обновление остатка поступления c id = {2} невозможно.", newQuantity, involvedQuantity.InvolvedQuantity, dto.Id);
				throw new ArgumentException(error, nameof(dto.Quantity));
			}
		}

		_logger.LogInformation("Проверка возможности изменения кол-ва поступления с id = {id} завершена успешно", dto.Id);

		var result = await _service.UpdateAsync(request.Dto).ConfigureAwait(false);

		if (result)
			await _mediator.Publish(new ReceiptUpdatedDomainEvent(request.Dto!.Id)).ConfigureAwait(false);

		return result;
	}
}