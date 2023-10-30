using StockControl.API.BackgroundTasks;
using Service.Common.Extensions;
using StockControl.API.BackgroundTask.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
	.AddDefaultSeqLog()
	.ConfigureServices((hostContext, services) =>
	{
		var configuration = hostContext.Configuration;

		services
			.AddRabbitMqEventBus(configuration)
			.AddIntegrationServices()
			.AddHandlers()
			.AddHostedService<EventPublisherHostedService>()
			.AddHostedService<DeadLetterHostedService>();
	})
	.Build();

await host.RunAsync();
