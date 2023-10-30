using MediatR;

using Microsoft.EntityFrameworkCore;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Events.Moving;
using StockControl.API.Services.Interfaces;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Moving.Deleted;

public class DeleteStockAvailabilityHandler : INotificationHandler<MovingDeletedDomainEvent>
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

	public async Task Handle(MovingDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		(Guid Id, Guid PartyId, int Quantity, Guid? SendingWarehouseId) remainderOfMoving = await _db.StockAvailabilities
			.Where(s => s.MovingId == @event.MovingId)
			.Where(s => !s.DeletedDate.HasValue)
			.Select(s => new { s.Id, s.PartyId, s.MovingId, s.Quantity })
			.Join(_db.Movings, sc => sc.MovingId, p => p.Id,
				(sc, p) => Tuple.Create(sc.Id, sc.PartyId, sc.Quantity, p.SendingWarehouseId).ToValueTuple())
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);

		var result = await _stocksService.DeleteAsync(remainderOfMoving.Id).ConfigureAwait(false);

		if (result)
		{
			await UpdateRemainderOfSenderWarehouseAsync(remainderOfMoving.Quantity, remainderOfMoving.SendingWarehouseId!.Value, remainderOfMoving.PartyId)
				.ConfigureAwait(false);
		}
	}

	private async Task UpdateRemainderOfSenderWarehouseAsync(int quantity, Guid sendingWarehouseId, Guid partyId)
	{
		_logger.LogInformation("Начинаем обновление остатка склада отправителя c id = {id} для партии {partyId}...", sendingWarehouseId, partyId);

		// получаем родителя
		var remainderOfSenderWarehouseDto = await _stocksService.GetRemainderOfSenderWarehouseAsync(sendingWarehouseId, partyId)
			.ConfigureAwait(false);

		if (remainderOfSenderWarehouseDto is null)
			throw new ArgumentNullException(nameof(remainderOfSenderWarehouseDto), "Не найден остаток склада отправителя");

		// обновляем кол-во на складе отправителе и обновляем остаток
		remainderOfSenderWarehouseDto.Quantity += quantity;

		await _stocksService.UpdateAsync(remainderOfSenderWarehouseDto).ConfigureAwait(false);

		_logger.LogInformation("Обновление остатка склада отправителя c id = {id} для партии {partyId} завершено успешно", sendingWarehouseId, partyId);
	}
}
