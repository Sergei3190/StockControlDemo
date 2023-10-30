using MediatR;

using StockControl.API.Domain.Events.WriteOff;
using StockControl.API.MediatR.Commands.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.WriteOff;

public class UpdateWriteOffCommandHandler : IRequestHandler<UpdateWriteOffCommand, bool>
{
	private readonly IWriteOffsService _service;
	private readonly ILogger<UpdateWriteOffCommandHandler> _logger;
	private readonly IPublisher _mediator;

	public UpdateWriteOffCommandHandler(IWriteOffsService service, ILogger<UpdateWriteOffCommandHandler> logger, IPublisher mediator)
	{
		_service = service;
		_logger = logger;
		_mediator = mediator;
	}

	public async Task<bool> Handle(UpdateWriteOffCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Dto;

		if (dto is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(CreateWriteOffCommand));
			throw new ArgumentNullException(error, nameof(dto));
		}

		var entity = await _service.GetByIdAsync(dto.Id).ConfigureAwait(false);

		if (entity is null)
			throw new ArgumentNullException("Списание не найдено в БД.", nameof(entity));

		// проверка того, что изменилось, в рамках демонстрации к редактированию доступны номер, цена, кол-во
		// акцент делаем на кол-ве
		if (dto.Quantity != entity.Quantity)
		{
			_logger.LogInformation("Начинаем проверку возможности изменения кол-ва списания с id = {id}..", dto.Id);

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
						" Обновление остатка списания c id = {2} невозможно.", newQuantity, involvedQuantity.InvolvedQuantity, dto.Id);
					throw new ArgumentException(error, nameof(dto.Quantity));
				}
			}

			_logger.LogInformation("Проверка возможности изменения кол-ва списания с id = {id} завершена успешно", dto.Id);
		}

		var result = await _service.UpdateAsync(request.Dto).ConfigureAwait(false);

		if (result)
			await _mediator.Publish(new WriteOffUpdatedDomainEvent(request.Dto!.Id)).ConfigureAwait(false);

		return result;
	}
}