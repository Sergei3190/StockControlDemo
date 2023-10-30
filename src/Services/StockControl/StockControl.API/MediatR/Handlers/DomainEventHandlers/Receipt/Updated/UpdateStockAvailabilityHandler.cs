using MediatR;

using Microsoft.EntityFrameworkCore;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Events.Receipt;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Services.Interfaces;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Receipt.Updated;

public class UpdateStockAvailabilityHandler : INotificationHandler<ReceiptUpdatedDomainEvent>
{
	private readonly StockControlDB _db;
	private readonly IReceiptsService _receiptsService;
	private readonly IStockAvailabilitiesService _stocksService;
	private readonly ILogger<UpdateStockAvailabilityHandler> _logger;

	public UpdateStockAvailabilityHandler(StockControlDB db, IReceiptsService receiptsService, IStockAvailabilitiesService stockService, ILogger<UpdateStockAvailabilityHandler> logger)
	{
		_db = db;
		_receiptsService = receiptsService;
		_stocksService = stockService;
		_logger = logger;
	}

	public async Task Handle(ReceiptUpdatedDomainEvent @event, CancellationToken cancellationToken)
	{
		var receiptDto = await _receiptsService.GetByIdAsync(@event.ReceiptId).ConfigureAwait(false);

		if (receiptDto is null)
		{
			var error = string.Format("Поступление с id: {0} не найдено в БД. Обновление остатка невозможно.", @event.ReceiptId);
			throw new ArgumentException(error, nameof(@event.ReceiptId));
		}

		var remainderOfReceiptId = await _db.StockAvailabilities
			.Where(s => s.ReceiptId == @event.ReceiptId)
			.Where(s => !s.DeletedDate.HasValue)
			.Select(s => s.Id)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);

		await _stocksService.UpdateAsync(receiptDto.CreateStockAvailabilityDto(remainderOfReceiptId)).ConfigureAwait(false);
	}
}