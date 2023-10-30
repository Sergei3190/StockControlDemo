using EventBus.Interfaces;

using EventBusRabbitMQ.Events;

using Identity.API.BackgroundTasks.Handlers;
using Identity.API.BackgroundTasks.Handlers.Interfaces;
using Identity.API.BackgroundTasks.Settings;

namespace Identity.API.BackgroundTask.Extensions;

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
