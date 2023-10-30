using MediatR;

using Microsoft.EntityFrameworkCore;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Events.Moving;
using StockControl.API.Services.Interfaces;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Moving.BulkDelete;

public class BulkDeleteStockAvailabilityHandler : INotificationHandler<MovingBulkDeletedDomainEvent>
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

	public async Task Handle(MovingBulkDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		IEnumerable<(Guid Id, Guid PartyId, int Quantity, Guid? SendingWarehouseId)> remainderOfMovings = await _db.StockAvailabilities
			.Where(s => @event.MovingIds.Contains(s.MovingId!.Value))
			.Where(s => !s.DeletedDate.HasValue)
			.Select(s => new { s.Id, s.PartyId, s.MovingId, s.Quantity })
			.Join(_db.Movings, sc => sc.MovingId, p => p.Id,
				(sc, p) => Tuple.Create(sc.Id, sc.PartyId, sc.Quantity, p.SendingWarehouseId).ToValueTuple())
			.ToArrayAsync()
			.ConfigureAwait(false);

		var result = await _stocksService.BulkDeleteAsync(remainderOfMovings.Select(r => r.Id).ToArray()).ConfigureAwait(false);

		if (result is null)
			throw new ArgumentNullException(nameof(result), "Получен недопустимый результат массового удаления");

		if (result.ErrorMessage!.Count == 0)
		{
			// TODO переделать на получение всей необходимой информации и последующее её обновление за минимальное кол-во обращение к БД по
			// аналогии с массовым удаление движений товара (поступление, списание, перемещение) и элементов справочника,
			foreach (var remainderOfMoving in remainderOfMovings)
			{
				await UpdateRemainderOfSenderWarehouseAsync(remainderOfMoving.Quantity, remainderOfMoving.SendingWarehouseId!.Value, remainderOfMoving.PartyId)
					.ConfigureAwait(false);
			}
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
