using System.Data.Common;

using EventBus.Interfaces;

using EventBusRabbitMQ.Events;

using Identity.API.DAL.Context;

using IntegrationEventLogEF.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

using Service.Common.Extensions;

namespace Identity.API.BackgroundTasks.Handlers;

public class DlxIntegrationEventHandler : IIntegrationEventHandler<DlxIntegrationEvent>
{
	private readonly IdentityDB _db;
	private readonly IExportIntegrationEventLogService _exportEventService;
	private readonly IConfiguration _configuration;
	private readonly ILogger<DlxIntegrationEventHandler> _logger;

	public DlxIntegrationEventHandler(
		Func<DbConnection, IExportIntegrationEventLogService> func,
		IdentityDB db,
		IConfiguration configuration,
		ILogger<DlxIntegrationEventHandler> logger)
	{
		_db = db ?? throw new ArgumentNullException(nameof(db));
		_exportEventService = func(_db.Database.GetDbConnection());
		_configuration = configuration;
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task Handle(DlxIntegrationEvent @event)
	{
		var integrationEvent = await _exportEventService.GetEventLogByEventIdAsync(@event.EventId).ConfigureAwait(false);

		// если кинем ошибку то это сообщение вновь может к нам попасть,
		// тк обменник недоставленных сообщений так настроен, поэтому если событие не нашли, то пишем лог и просто выходим из обработки
		if (integrationEvent is null)
		{
			_logger.LogWarning("В БД не найдено полученное на обработку интеграционное событие {EventId}, " +
				"отбрасываем его из обменника недоставленных интеграционных событий...", @event.EventId);

			return;
		}

		string errorMessage = null!;
		var maxTimeSent = Convert.ToInt32(_configuration.GetRequiredValue("Workers:EventPublisher:MaxTimesSent"));

		if (integrationEvent.TimesSent > maxTimeSent)
			errorMessage = string.Format("Превышено максимальное кол-во повторных отправок: " +
				"TimesSent {0} MaxTimeSent {1}", integrationEvent.TimesSent, maxTimeSent);

		await _exportEventService.MarkEventAsFailedAsync(eventId: @event.EventId, error: errorMessage);
	}
}