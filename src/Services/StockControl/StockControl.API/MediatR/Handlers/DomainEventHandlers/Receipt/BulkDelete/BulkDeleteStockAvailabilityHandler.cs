using MediatR;

using Microsoft.EntityFrameworkCore;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Events.Receipt;
using StockControl.API.Services.Interfaces;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Receipt.BulkDelete;

public class BulkDeleteStockAvailabilityHandler : INotificationHandler<ReceiptBulkDeletedDomainEvent>
{
	private readonly StockControlDB _db;
	private readonly IStockAvailabilitiesService _stocksService;
	private readonly ILogger<BulkDeleteStockAvailabilityHandler> _logger;

	public BulkDeleteStockAvailabilityHandler(StockControlDB db, IStockAvailabilitiesService stockService, ILogger<BulkDeleteStockAvailabilityHandler> logger)
	{
		_db = db;
		_stocksService = stockService;
		_logger = logger;
	}

	public async Task Handle(ReceiptBulkDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		var remainderOfReceiptIds = await _db.StockAvailabilities
			.Where(s => @event.ReceiptIds.Contains(s.ReceiptId!.Value))
			.Where(s => !s.DeletedDate.HasValue)
			.Select(s => s.Id)
			.ToArrayAsync()
			.ConfigureAwait(false);

		if (!remainderOfReceiptIds.Any())
			return;

		await _stocksService.BulkDeleteAsync(remainderOfReceiptIds).ConfigureAwait(false);
	}
}