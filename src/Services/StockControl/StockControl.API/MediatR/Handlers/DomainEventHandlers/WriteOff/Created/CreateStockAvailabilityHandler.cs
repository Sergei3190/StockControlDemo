using MediatR;

using StockControl.API.Domain.Events.WriteOff;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Services.Interfaces;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.WriteOff.Created;

public class CreateStockAvailabilityHandler : INotificationHandler<WriteOffCreatedDomainEvent>
{
	private readonly IWriteOffsService _writeOffsService;
	private readonly IStockAvailabilitiesService _stocksService;
	private readonly ILogger<CreateStockAvailabilityHandler> _logger;

	public CreateStockAvailabilityHandler(IWriteOffsService writeOffsService, IStockAvailabilitiesService stockService,
		ILogger<CreateStockAvailabilityHandler> logger)
	{
		_writeOffsService = writeOffsService;
		_stocksService = stockService;
		_logger = logger;
	}

	public async Task Handle(WriteOffCreatedDomainEvent @event, CancellationToken cancellationToken)
	{
		var writeOffDto = await _writeOffsService.GetByIdAsync(@event.WriteOffId).ConfigureAwait(false);

		if (writeOffDto is null)
		{
			var error = string.Format("Списание с id: {0} не найдено в БД. Создание остатка списания невозможно.", @event.WriteOffId);
			throw new ArgumentException(error, nameof(@event.WriteOffId));
		}

		if (writeOffDto.SendingWarehouse is null)
			throw new ArgumentNullException("Не заполнен склад отправитель. Создание остатка списания невозможно.", nameof(writeOffDto.SendingWarehouse));

		// получаем наличие товара на складе отправителе по партии
		// партия уникальна в рамках одного склада
		var remainderOfSenderWarehouseDto = await _stocksService.GetRemainderOfSenderWarehouseAsync(writeOffDto.SendingWarehouse.Id, writeOffDto.Party!.Id)
			.ConfigureAwait(false);

		if (remainderOfSenderWarehouseDto is null)
			throw new ArgumentNullException(nameof(remainderOfSenderWarehouseDto), "Не найден остаток склада отправителя");

		var newQuantity = writeOffDto.Quantity;

		if (remainderOfSenderWarehouseDto.Quantity < newQuantity)
		{
			var error = string.Format("Не хватает товара в кол-ве {0}. Создание остатка списания невозможно.", newQuantity - remainderOfSenderWarehouseDto.Quantity);
			throw new ArgumentException(error, nameof(@event.WriteOffId));
		}

		// если хватает, то создаём остаток на новом складе
		await _stocksService.CreateAsync(writeOffDto.CreateStockAvailabilityDto()).ConfigureAwait(false);

		// уменьшаем кол-во на складе отправителе и обновляем остаток
		remainderOfSenderWarehouseDto.Quantity -= newQuantity;
		await _stocksService.UpdateAsync(remainderOfSenderWarehouseDto).ConfigureAwait(false);
	}
}