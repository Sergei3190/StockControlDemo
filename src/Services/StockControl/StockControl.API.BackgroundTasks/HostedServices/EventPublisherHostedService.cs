using StockControl.API.BackgroundTasks.Handlers.Interfaces;

namespace StockControl.API.BackgroundTasks;

/// <summary>
/// ������� ������ ���������� �������������� �������
/// </summary>
public class EventPublisherHostedService : BackgroundService
{
	private readonly ILogger<EventPublisherHostedService> _logger;
	private readonly IServiceProvider _serviceProvider;

	public EventPublisherHostedService(ILogger<EventPublisherHostedService> logger,
		IServiceProvider serviceProvider)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("������ {EventPublisherHostedService} � {time}", typeof(EventPublisherHostedService).FullName, DateTimeOffset.Now.ToLocalTime());

		stoppingToken.Register(() => _logger.LogInformation("#EP1 ��������� ������ {EventPublisherHostedService} � {time}", typeof(EventPublisherHostedService).FullName,
			DateTimeOffset.Now.ToLocalTime()));

	    await DoWorkAsync(stoppingToken).ConfigureAwait(false);
	}

	private async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		await using (var scope = _serviceProvider.CreateAsyncScope())
		{
			var handler = scope.ServiceProvider.GetRequiredService<IEventPublisherHandler>();

			// ������ �������� CancellationTokenSource, ��� ����� ������ ���������, ���� ��������� ������������ ����� ������
			var childCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);

			await handler.HandleAsync(_serviceProvider, childCts.Token).ConfigureAwait(false);
		}
	}
}