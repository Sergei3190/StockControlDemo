using MediatR;

using Microsoft.EntityFrameworkCore;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Events.WriteOff;
using StockControl.API.Services.Interfaces;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.WriteOff.Deleted;

public class DeleteStockAvailabilityHandler : INotificationHandler<WriteOffDeletedDomainEvent>
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

	public async Task Handle(WriteOffDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		(Guid Id, Guid PartyId, int Quantity, Guid? SendingWarehouseId) remainderOfWriteOff = await _db.StockAvailabilities
			.Where(s => s.WriteOffId == @event.WriteOffId)
			.Where(s => !s.DeletedDate.HasValue)
			.Select(s => new { s.Id, s.PartyId, s.WriteOffId, s.Quantity })
			.Join(_db.WriteOffs, sc => sc.WriteOffId, p => p.Id,
				(sc, p) => Tuple.Create(sc.Id, sc.PartyId, sc.Quantity, p.SendingWarehouseId).ToValueTuple())
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);

		var result = await _stocksService.DeleteAsync(remainderOfWriteOff.Id).ConfigureAwait(false);

		if (result)
		{
			await UpdateRemainderOfSenderWarehouseAsync(remainderOfWriteOff.Quantity, remainderOfWriteOff.SendingWarehouseId!.Value, remainderOfWriteOff.PartyId)
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
