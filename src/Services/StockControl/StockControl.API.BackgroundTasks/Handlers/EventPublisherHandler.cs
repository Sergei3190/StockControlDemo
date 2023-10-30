using EventBus.Interfaces;

using IntegrationEventLogEF;
using IntegrationEventLogEF.Dapper;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

using Polly;
using Polly.Retry;

using StockControl.API.BackgroundTask.Extensions;
using StockControl.API.BackgroundTasks.Handlers.Interfaces;
using StockControl.API.BackgroundTasks.Settings;

namespace StockControl.API.BackgroundTasks.Handlers;

public class EventPublisherHandler : IEventPublisherHandler
{
	private readonly IExportIntegrationEventLogDapperService _exportEventService;
	private readonly IEventBus _eventBus;
	private readonly ILogger<EventPublisherHandler> _logger;

	private EventPublisherSettings _eventPublisherSettings;

	public EventPublisherHandler(
		IConfiguration configuration,
		Func<string, IExportIntegrationEventLogDapperService> func,
		IEventBus eventBus,
		ILogger<EventPublisherHandler> logger)
	{
		_exportEventService = func(configuration.GetDbConnectionString());
		_eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task HandleAsync(IServiceProvider provider, CancellationToken cancel)
	{
		_logger.LogInformation("Запуск {EventPublisherHandler} в {time}", typeof(EventPublisherHandler).FullName, DateTimeOffset.Now.ToLocalTime());

		cancel.Register(() => _logger.LogInformation("#EP2 Завершаем работу {EventPublisherHandler} в {time}",
			typeof(EventPublisherHandler).FullName, DateTimeOffset.Now.ToLocalTime()));

		await DoWorkAsync(provider, cancel).ConfigureAwait(false);
	}

	private async Task DoWorkAsync(IServiceProvider provider, CancellationToken token)
	{
		while (!token.IsCancellationRequested)
		{
			try
			{
				await using (var scope = provider.CreateAsyncScope())
				{
					// получаем актуальные настройки для выборки и засыпания из конфига
					var options = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<EventPublisherSettings>>();
					_eventPublisherSettings = options.Value;
				}

				// задаём начальный фильтр для циклической выборки данных для публикации
				var filter = GetEventLogFilter();

				var retryPolicy = await GetRetryPolicyAsync().ConfigureAwait(false);

				await retryPolicy!.ExecuteAsync(async () =>
				{
					var executionCode = await PublishAsync(provider, filter, token).ConfigureAwait(false);
					_logger.LogTrace("Публикация завершилась с кодом {executionCode}. Спим {Delay} секунд(у)...", executionCode, _eventPublisherSettings.Delay);
				});

				await Task.Delay(TimeSpan.FromSeconds(_eventPublisherSettings.Delay), token);
			}
			catch (Exception ex)
			{
				_logger.LogError("Произошла ошибка во время работы воркера {StockControl.API.BackgroundTasks}. ОШИБКА: {0}", ex.Message);
				await Task.Delay(TimeSpan.FromSeconds(_eventPublisherSettings.Delay), token);
			}
		}
	}

	private async Task<int> PublishAsync(IServiceProvider provider, EventLogFilterDto filter, CancellationToken token)
	{
		var count = await _exportEventService.GetCountAsync(filter.States!).ConfigureAwait(false);

		if (count == 0)
			return 0;

		do
		{
			var integrationEvents = await _exportEventService.GetEventLogsAsync(filter).ConfigureAwait(false);

			foreach (var logEvt in integrationEvents)
			{
				_logger.LogInformation("Публикации интеграционного события: {IntegrationEventId} - ({@IntegrationEvent})", logEvt.EventId, logEvt.IntegrationEvent);

				try
				{
					await _exportEventService.MarkEventAsInProgressAsync(logEvt.EventId);
					_eventBus.Publish(logEvt.IntegrationEvent!);
					await _exportEventService.MarkEventAsPublishedAsync(logEvt.EventId);
				}
				catch (Exception ex)
				{
					_logger.LogError(ex, "Ошибка публикации интеграционного события {IntegrationEventId}", logEvt.EventId);
					await _exportEventService.MarkEventAsFailedAsync(logEvt.EventId);
					return 1; // что то пошло не так
				}
			}

			filter.Page += 1;
			count -= integrationEvents.Count();

		} while (count > 0);

		return 0; // успех
	}

	private EventLogFilterDto GetEventLogFilter()
	{
		return new EventLogFilterDto(page: _eventPublisherSettings.Page,
			pageSize: _eventPublisherSettings.PageSize,
			maxTimeSent: _eventPublisherSettings.MaxTimesSent,
			states: new EventStateEnum[] { EventStateEnum.NotPublished, EventStateEnum.PublishedFailed });
	}

	// https://github.com/App-vNext/Polly#wait-and-retry
	// https://stackoverflow.com/questions/73095394/polly-waitandretryasync-vs-waitandretry
	private async Task<AsyncRetryPolicy?> GetRetryPolicyAsync()
	{
		return Policy
		 .Handle<InvalidOperationException>()
		 .Or<SqlException>()
		 .WaitAndRetryAsync(_eventPublisherSettings.RetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(_eventPublisherSettings.RetryTimeout, retryAttempt)), (ex, time) =>
		 {
			 _logger.LogWarning(ex, "Не удалось установить соединение с БД StockControlDB после {Timeout}s", $"{time.TotalSeconds:n1}");
		 });
	}
}