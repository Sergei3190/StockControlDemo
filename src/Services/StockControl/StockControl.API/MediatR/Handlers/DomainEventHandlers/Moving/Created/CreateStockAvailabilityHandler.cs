using MediatR;

using StockControl.API.Domain.Events.Moving;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Services.Interfaces;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Moving.Created;

public class CreateStockAvailabilityHandler : INotificationHandler<MovingCreatedDomainEvent>
{
	private readonly IMovingsService _movingsService;
	private readonly IStockAvailabilitiesService _stocksService;
	private readonly ILogger<CreateStockAvailabilityHandler> _logger;

	public CreateStockAvailabilityHandler(IMovingsService movingsService, IStockAvailabilitiesService stockService,
		ILogger<CreateStockAvailabilityHandler> logger)
	{
		_movingsService = movingsService;
		_stocksService = stockService;
		_logger = logger;
	}

	public async Task Handle(MovingCreatedDomainEvent @event, CancellationToken cancellationToken)
	{
		var movingDto = await _movingsService.GetByIdAsync(@event.MovingId).ConfigureAwait(false);

		if (movingDto is null)
		{
			var error = string.Format("Перемещение с id: {0} не найдено в БД. Создание остатка перемещения невозможно.", @event.MovingId);
			throw new ArgumentException(error, nameof(@event.MovingId));
		}

		if (movingDto.SendingWarehouse is null)
			throw new ArgumentNullException("Не заполнен склад отправитель. Создание остатка перемещения невозможно.", nameof(movingDto.SendingWarehouse));

		// получаем наличие товара на складе отправителе по партии
		// партия уникальна в рамках одного склада
		var remainderOfSenderWarehouseDto = await _stocksService.GetRemainderOfSenderWarehouseAsync(movingDto.SendingWarehouse.Id, movingDto.Party!.Id)
			.ConfigureAwait(false);

		if (remainderOfSenderWarehouseDto is null)
			throw new ArgumentNullException(nameof(remainderOfSenderWarehouseDto), "Не найден остаток склада отправителя");

		var newQuantity = movingDto.Quantity;

		if (remainderOfSenderWarehouseDto.Quantity < newQuantity)
		{
			var error = string.Format("Не хватает товара в кол-ве {0}. Создание остатка перемещения невозможно.", newQuantity - remainderOfSenderWarehouseDto.Quantity);
			throw new ArgumentException(error, nameof(@event.MovingId));
		}

		// если хватает, то создаём остаток на новом складе
		await _stocksService.CreateAsync(movingDto.CreateStockAvailabilityDto()).ConfigureAwait(false);

		// уменьшаем кол-во на складе отправителе и обновляем остаток
		remainderOfSenderWarehouseDto.Quantity -= newQuantity;
		await _stocksService.UpdateAsync(remainderOfSenderWarehouseDto).ConfigureAwait(false);
	}
}