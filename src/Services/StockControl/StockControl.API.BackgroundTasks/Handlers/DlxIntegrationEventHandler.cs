using EventBus.Interfaces;

using EventBusRabbitMQ.Events;

using IntegrationEventLogEF.Dapper;

using Service.Common.Extensions;

using StockControl.API.BackgroundTask.Extensions;

namespace StockControl.API.BackgroundTasks.Handlers;

public class DlxIntegrationEventHandler : IIntegrationEventHandler<DlxIntegrationEvent>
{
	private readonly IConfiguration _configuration;
	private readonly IExportIntegrationEventLogDapperService _exportEventService;
	private readonly ILogger<DlxIntegrationEventHandler> _logger;

	public DlxIntegrationEventHandler(
		IConfiguration configuration,
		Func<string, IExportIntegrationEventLogDapperService> func,
		ILogger<DlxIntegrationEventHandler> logger)
	{
		_configuration = configuration;
		_exportEventService = func(_configuration.GetDbConnectionString());
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task Handle(DlxIntegrationEvent @event)
	{
		var integrationEvent = await _exportEventService.GetEventLogByEventIdAsync(@event.EventId).ConfigureAwait(false);

		// если кинем ошибку то это сообщение вновь может к нам попасть, тк обменник недоставленных сообщений так настрое, поэтому если событие не нашли, то пишем лог 
		// и просто выходим из обработки
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

		await _exportEventService.MarkEventAsFailedAsync(eventId: @event.EventId, error: errorMessage).ConfigureAwait(false);
	}
}