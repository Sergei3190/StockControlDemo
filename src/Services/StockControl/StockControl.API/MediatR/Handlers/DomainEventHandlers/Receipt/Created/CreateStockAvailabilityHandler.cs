using MediatR;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Services.Interfaces;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Receipt.Created;

public class CreateStockAvailabilityHandler : INotificationHandler<ReceiptCreatedDomainEvent>
{
	private readonly IReceiptsService _receiptsService;
	private readonly IStockAvailabilitiesService _stocksService;
	private readonly ILogger<CreateStockAvailabilityHandler> _logger;

	public CreateStockAvailabilityHandler(IReceiptsService receiptsService, IStockAvailabilitiesService stockService, ILogger<CreateStockAvailabilityHandler> logger)
	{
		_receiptsService = receiptsService;
		_stocksService = stockService;
		_logger = logger;
	}

	public async Task Handle(ReceiptCreatedDomainEvent @event, CancellationToken cancellationToken)
	{
		var receiptDto = await _receiptsService.GetByIdAsync(@event.ReceiptId).ConfigureAwait(false);

		if (receiptDto is null)
		{
			var error = string.Format("Поступление с id: {0} не найдено в БД. Создание остатка невозможно.", @event.ReceiptId);
			throw new ArgumentException(error, nameof(@event.ReceiptId));
		}

		await _stocksService.CreateAsync(receiptDto.CreateStockAvailabilityDto()).ConfigureAwait(false);
	}
}