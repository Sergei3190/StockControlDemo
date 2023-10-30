using EventBus.Interfaces;

using EventBusRabbitMQ.Events;
using EventBusRabbitMQ.Interfaces;

namespace StockControl.API.BackgroundTasks;

/// <summary>
/// ������� ������ ��������� �������������� �������������� �������
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
        _logger.LogInformation("������ {DeadLetterHostedService} � {time}", typeof(DeadLetterHostedService).FullName, DateTimeOffset.Now.ToLocalTime());

        stoppingToken.Register(() => _logger.LogInformation("#DL1 ��������� ������ {DeadLetterHostedService} � {time}", 
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
                _logger.LogInformation("�� ������ ������ {IEventBusDeadLetter}, �������� ���� ������� ���������.. ��������� ��������� �������..", typeof(IEventBusDeadLetter));
                return;
            }

            eventBus.Subscribe<DlxIntegrationEvent, IIntegrationEventHandler<DlxIntegrationEvent>>();
            _logger.LogInformation("����������� �������� �� �������������� ������� {DlxIntegrationEvent}", typeof(DlxIntegrationEvent));
        }
    }
}