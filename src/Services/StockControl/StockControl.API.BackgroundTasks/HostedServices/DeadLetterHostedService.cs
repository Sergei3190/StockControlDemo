using EventBus.Interfaces;

using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Interfaces;

namespace StockControl.API.BackgroundTasks;

/// <summary>
/// Фоновая задача обработки недоставленных интеграционных событий
/// </summary>
public class DeadLetterHostedService : BackgroundService
{
    private readonly ILogger<DeadLetterHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public DeadLetterHostedService(ILogger<DeadLetterHostedService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Запуск {DeadLetterHostedService} в {time}", typeof(DeadLetterHostedService).FullName, DateTimeOffset.Now.ToLocalTime());

        stoppingToken.Register(() => _logger.LogInformation("#DL1 Завершаем работу {DeadLetterHostedService} в {time}", 
            typeof(DeadLetterHostedService).FullName, DateTimeOffset.Now.ToLocalTime()));

        await DoWorkAsync().ConfigureAwait(false);
    }

    private async Task DoWorkAsync()
    {
        await using (var scope = _serviceProvider.CreateAsyncScope())
        {
            var eventBus = scope.ServiceProvider.GetService<IEventBusDeadLetter>();

            if (eventBus is null)
            {
                _logger.LogInformation("Не найден сервис {IEventBusDeadLetter}, возможно шина событий отключена.. Проверьте настройки конфига..", typeof(IEventBusDeadLetter));
                return;
            }

            eventBus.Subscribe<DlxIntegrationEvent, IIntegrationEventHandler<DlxIntegrationEvent>>();
            _logger.LogInformation("Установлена подписка на интеграционное событие {DlxIntegrationEvent}", typeof(DlxIntegrationEvent));
        }
    }
}