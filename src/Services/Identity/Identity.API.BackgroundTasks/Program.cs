using Identity.API.BackgroundTask.Extensions;
using Identity.API.BackgroundTasks;

using Service.Common.Extensions;

IHost host = Host.CreateDefaultBuilder(args)
	.AddDefaultSeqLog()
	.ConfigureServices((hostContext, services) =>
	{
		var configuration = hostContext.Configuration;

		services
			.AddIdentityDbContext(configuration)
			.AddRabbitMqEventBus(configuration)
			.AddIntegrationServices()
			.AddHandlers()
			.AddHostedService<EventPublisherHostedService>()
			.AddHostedService<DeadLetterHostedService>();
	})
	.Build();

await host.RunAsync();
