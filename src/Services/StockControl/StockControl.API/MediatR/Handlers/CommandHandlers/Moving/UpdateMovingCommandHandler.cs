using MediatR;

using StockControl.API.Domain.Events.Moving;
using StockControl.API.MediatR.Commands.Moving;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.Moving;

public class UpdateMovingCommandHandler : IRequestHandler<UpdateMovingCommand, bool>
{
	private readonly IMovingsService _service;
	private readonly ILogger<UpdateMovingCommandHandler> _logger;
	private readonly IPublisher _mediator;

	public UpdateMovingCommandHandler(IMovingsService service, ILogger<UpdateMovingCommandHandler> logger, IPublisher mediator)
	{
		_service = service;
		_logger = logger;
		_mediator = mediator;
	}

	public async Task<bool> Handle(UpdateMovingCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Dto;

		if (dto is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(CreateMovingCommand));
			throw new ArgumentNullException(error, nameof(dto));
		}

		var entity = await _service.GetByIdAsync(dto.Id).ConfigureAwait(false);

		if (entity is null)
			throw new ArgumentNullException("Перемещение не найдено в БД.", nameof(entity));

		// проверка того, что изменилось, в рамках демонстрации к редактированию доступны номер, цена, кол-во
		// акцент делаем на кол-ве
		if (dto.Quantity != entity.Quantity)
		{
			_logger.LogInformation("Начинаем проверку возможности изменения кол-ва перемещения с id = {id}..", dto.Id);

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
						" Обновление остатка перемещения c id = {2} невозможно.", newQuantity, involvedQuantity.InvolvedQuantity, dto.Id);
					throw new ArgumentException(error, nameof(dto.Quantity));
				}
			}

			_logger.LogInformation("Проверка возможности изменения кол-ва перемещения с id = {id} завершена успешно", dto.Id);
		}

		var result = await _service.UpdateAsync(request.Dto).ConfigureAwait(false);

		if (result)
			await _mediator.Publish(new MovingUpdatedDomainEvent(request.Dto!.Id)).ConfigureAwait(false);

		return result;
	}
}