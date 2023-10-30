using EventBus;
using EventBus.Interfaces;

using EventBusRabbitMQ;
using EventBusRabbitMQ.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RabbitMQ.Client;

using Service.Common.Integration.Settings;

namespace Service.Common.Extensions;

public static class EventBusExtension
{
	public static IServiceCollection AddRabbitMqEventBus(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		var eventBusSection = configuration.GetSection("EventBus");

		if (!eventBusSection.Exists())
			return services;

		var eventBusSetting = eventBusSection.Get<EventBusSetting>()!;

		// чтобы можно было обращаться из приложения к настройкам
		services.Configure<EventBusSetting>(eventBusSection)
			.AddSingleton(eventBusSetting);

		//Если шина выключена, вставляем затычку.
		if (eventBusSetting is null || !eventBusSetting.Enabled)
		{
			services.AddSingleton<IEventBus, EmptyEventBus>();

			return services;
		}

		services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
		{
			string GetHost(string connect)
			{
				switch (connect)
				{
					case nameof(eventBusSetting.BusAccess.DockerHost):
						return eventBusSetting!.BusAccess.DockerHost;
					default:
						return eventBusSetting!.BusAccess.Host;
				}
			};

			var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();

			var host = GetHost(eventBusSetting.Connect);

			var factory = new ConnectionFactory()
			{
				HostName = host,
				DispatchConsumersAsync = true
			};

			factory.UserName = eventBusSetting.BusAccess.UserName;
			factory.Password = eventBusSetting.BusAccess.Password;

			return new DefaultRabbitMqPersistentConnection(factory, logger, eventBusSetting.BusAccess.RetryCount);
		});

		services.AddSingleton<IEventBusSubscriptionsManager, InMemorySubscriptionsManager>();

		if (eventBusSetting!.DeadLetter != null && !eventBusSetting.DeadLetter.Enabled)
		{
			services.AddSingleton<IEventBusDeadLetter, EmptyEventBusDeadLetter>();
		}
		else
		{
			services.AddSingleton<IEventBusDeadLetter, EventBusDeadLetter>(sp =>
			{
				var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
				var logger = sp.GetRequiredService<ILogger<EventBusDeadLetter>>();
				var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

				return new EventBusDeadLetter(rabbitMQPersistentConnection, logger, sp, eventBusSubscriptionsManager, eventBusSetting.DeadLetter);
			});
		}

		services.AddSingleton<IEventBus, EventBusDefault>(sp =>
		{
			var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
			var logger = sp.GetRequiredService<ILogger<EventBusDefault>>();
			var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
			var dlxEventBus = sp.GetRequiredService<IEventBusDeadLetter>();

			return new EventBusDefault(rabbitMQPersistentConnection, logger, sp, eventBusSubscriptionsManager, dlxEventBus, eventBusSetting.Default);
		});

		return services;
	}
}
