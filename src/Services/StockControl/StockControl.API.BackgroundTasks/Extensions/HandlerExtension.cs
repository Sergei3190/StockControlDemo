using EventBus.Interfaces;

using EventBusRabbitMQ.Events;

using StockControl.API.BackgroundTasks.Handlers;
using StockControl.API.BackgroundTasks.Handlers.Interfaces;
using StockControl.API.BackgroundTasks.Settings;

namespace StockControl.API.BackgroundTask.Extensions;

public static class HandlerExtension
{
	public static IServiceCollection AddHandlers(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddTransient<IEventPublisherHandler, EventPublisherHandler>()
			.AddOptions<EventPublisherSettings>().BindConfiguration("Workers:EventPublisher");

		services.AddTransient<IIntegrationEventHandler<DlxIntegrationEvent>, DlxIntegrationEventHandler>();

		return services;
	}
}
