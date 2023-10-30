using System.Data.Common;

using EventBus.Interfaces;

using IntegrationEventLogEF;

using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

using Notification.API.DAL.Context;
using Notification.API.SignalR.Hubs;

using Service.Common.Integration.Events.StockControl.WriteOff;

namespace Notification.API.IntegrationEvenHandlers.StockControl.WriteOff;

public class WriteOffCreatedIntegrationEventHandler : IIntegrationEventHandler<WriteOffCreatedIntegrationEvent>
{
	private readonly IHubContext<NotificationHub> _hubContext;
	private readonly NotificationDB _db;
	private readonly IImportSuccessIntegrationEventLogService _importEventService;
	private readonly ILogger<WriteOffCreatedIntegrationEventHandler> _logger;

	private readonly string _group;

	public WriteOffCreatedIntegrationEventHandler(
		IHubContext<NotificationHub> hubContext,
		Func<DbConnection, IImportSuccessIntegrationEventLogService> integrationFunc,
		NotificationDB db,
		IConfiguration configuration,
		Func<IConfiguration, string> groupFunc,
		ILogger<WriteOffCreatedIntegrationEventHandler> logger)
	{
		_hubContext = hubContext;
		_db = db;
		_importEventService = integrationFunc(_db.Database.GetDbConnection());
		_group = groupFunc(configuration);
		_logger = logger;
	}

	public async Task Handle(WriteOffCreatedIntegrationEvent @event)
	{
		// сначала проверяем обрабатывали ли мы полученное событие или может быть к нам пришло событие с неактуальной информацией, те дата его меньше чем дата, 
		// ранее обработанного события
		var handledEvent = await _importEventService.CheckIsExistEventLogAsync(
			new EventLogDto(
				EventId: @event.Id,
				CreationTime: @event.CreationDate,
				EventTypeName: typeof(WriteOffCreatedIntegrationEvent).FullName!)
			)
			.ConfigureAwait(false);

		// если true, то нам не интересно, откидываем событие
		if (handledEvent)
			return;

		// сначала сохраняем интеграционного события, если будет ошибка, то её обработает уже шина сообщений,
		// а уже после сохранения отправляем уведомление клиентам, тк если сначала отправлять уведомление клиентам, а потом сохранять событие, то при 
		// возникновении ошибки сохранения, сообщение будет повторно отправлено в очередь публикации и придя сюда повторно отправит уведомление,
		// но в нашем случаи если ошибка будет только на этапе отправки уведомления, то все изменения в бд откатятся и мы не будем дудосить 
		// клиента повторными уведомлениями
		await ResilientTransaction.New(_db).ExecuteAsync(async () =>
		{
			await _db.SaveChangesAsync();
			await _importEventService.SaveEventAsync(@event, _db.Database.CurrentTransaction!);
		})
			.ConfigureAwait(false);

		_logger.LogInformation("Отправляем уведомление (id = {id}) создания списания клиентам..", @event.Id);

		var connectionIds = NotificationHub.UserConnections
			.Where(c => c.Value == @event.CreatorName)
			.Select(c => c.Key);

		// вызывает метод у клиентов группы по имени groupName за исключением тех клиентов, id которых передаются в качестве второго параметра
		await _hubContext.Clients.GroupExcept(_group, connectionIds).SendAsync("WriteOffCreated", new { @event.ProductFlowId, @event.Number });

		_logger.LogInformation("Отправка уведомления (id = {id}) создания списания клиентам выполнена успешно.", @event.Id);
	}
}
