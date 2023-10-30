using MediatR;

using Microsoft.EntityFrameworkCore;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Events.Receipt;
using StockControl.API.Services.Interfaces;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Receipt.Deleted;

public class DeleteStockAvailabilityHandler : INotificationHandler<ReceiptDeletedDomainEvent>
{
	private readonly StockControlDB _db;
	private readonly IStockAvailabilitiesService _stocksService;
	private readonly ILogger<DeleteStockAvailabilityHandler> _logger;

	public DeleteStockAvailabilityHandler(StockControlDB db, IStockAvailabilitiesService stockService, ILogger<DeleteStockAvailabilityHandler> logger)
	{
		_db = db;
		_stocksService = stockService;
		_logger = logger;
	}

	public async Task Handle(ReceiptDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		var remainderOfReceiptId = await _db.StockAvailabilities
			.Where(s => s.ReceiptId == @event.ReceiptId)
			.Where(s => !s.DeletedDate.HasValue)
			.Select(s => s.Id)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);

		await _stocksService.DeleteAsync(remainderOfReceiptId).ConfigureAwait(false);
	}
}