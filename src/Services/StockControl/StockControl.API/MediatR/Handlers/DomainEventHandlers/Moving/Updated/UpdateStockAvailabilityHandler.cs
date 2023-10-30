using MediatR;

using Microsoft.EntityFrameworkCore;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Events.Moving;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Services.Interfaces;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Moving.Updated;

public class UpdateStockAvailabilityHandler : INotificationHandler<MovingUpdatedDomainEvent>
{
	private readonly StockControlDB _db;
	private readonly IMovingsService _movingsService;
	private readonly IStockAvailabilitiesService _stocksService;
	private readonly ILogger<UpdateStockAvailabilityHandler> _logger;

	public UpdateStockAvailabilityHandler(StockControlDB db, IMovingsService movingsService, IStockAvailabilitiesService stockService, ILogger<UpdateStockAvailabilityHandler> logger)
	{
		_db = db;
		_movingsService = movingsService;
		_stocksService = stockService;
		_logger = logger;
	}

	public async Task Handle(MovingUpdatedDomainEvent @event, CancellationToken cancellationToken)
	{
		var movingDto = await _movingsService.GetByIdAsync(@event.MovingId).ConfigureAwait(false);

		if (movingDto is null)
		{
			var error = string.Format("Перемещения с id: {0} не найдено в БД. Обновление остатка невозможно.", @event.MovingId);
			throw new ArgumentException(error, nameof(@event.MovingId));
		}

		(Guid Id, int Quantity) currentRemainderOfMoving = await _db.StockAvailabilities
			.Where(s => s.MovingId == @event.MovingId)
			.Where(s => !s.DeletedDate.HasValue)
			.Select(s => Tuple.Create(s.Id, s.Quantity).ToValueTuple())
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);

		await UpdateRemainderOfSenderWarehouseAsync(currentRemainderOfMoving.Quantity, movingDto.Quantity, movingDto.SendingWarehouse!.Id, movingDto.Party!.Id)
			.ConfigureAwait(false);

		await _stocksService.UpdateAsync(movingDto.CreateStockAvailabilityDto(currentRemainderOfMoving.Id)).ConfigureAwait(false);
	}

	private async Task UpdateRemainderOfSenderWarehouseAsync(int currentQuantity, int newQuantity, Guid sendingWarehouseId, Guid partyId)
	{
		_logger.LogInformation("Начинаем обновление остатка склада отправителя c id = {id} для партии {partyId}...", sendingWarehouseId, partyId);

		// нам надо проверить хватает ли остатка у родителя нашего перемещения
		// получаем наличие товара на складе отправителе по партии
		// партия уникальна в рамках одного склада
		var remainderOfSenderWarehouseDto = await _stocksService.GetRemainderOfSenderWarehouseAsync(sendingWarehouseId, partyId)
			.ConfigureAwait(false);

		if (remainderOfSenderWarehouseDto is null)
			throw new ArgumentNullException(nameof(remainderOfSenderWarehouseDto), "Не найден остаток склада отправителя");

		var differenceQuantity = (currentQuantity - newQuantity);

		if (differenceQuantity >= 0)
			remainderOfSenderWarehouseDto.Quantity += differenceQuantity;

		if (remainderOfSenderWarehouseDto.Quantity < (-1 * differenceQuantity))
		{
			var error = string.Format("Не хватает товара в кол-ве {0}. Обновление остатка перемещения невозможно.", 
				(-1 * differenceQuantity) - remainderOfSenderWarehouseDto.Quantity);
			throw new ArgumentException(error, nameof(newQuantity));
		}

		remainderOfSenderWarehouseDto.Quantity -= differenceQuantity >= 0 ? differenceQuantity : (-1 * differenceQuantity);

		await _stocksService.UpdateAsync(remainderOfSenderWarehouseDto).ConfigureAwait(false);

		_logger.LogInformation("Обновление остатка склада отправителя c id = {id} для партии {partyId} завершено успешно", sendingWarehouseId, partyId);
	}
}