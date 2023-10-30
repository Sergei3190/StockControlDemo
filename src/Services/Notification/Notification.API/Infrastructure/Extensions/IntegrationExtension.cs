using System.Data.Common;

using EventBus.Interfaces;

using IntegrationEventLogEF;

using Notification.API.IntegrationEvenHandlers.Identity;
using Notification.API.IntegrationEvenHandlers.StockControl.Moving;
using Notification.API.IntegrationEvenHandlers.StockControl.Receipt;
using Notification.API.IntegrationEvenHandlers.StockControl.WriteOff;

using Service.Common.Integration.Events.Identity;
using Service.Common.Integration.Events.StockControl.Moving;
using Service.Common.Integration.Events.StockControl.Receipt;
using Service.Common.Integration.Events.StockControl.WriteOff;

namespace Notification.API.Infrastructure.Extensions;

public static class IntegrationExtension
{
	public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddTransient<Func<DbConnection, IImportSuccessIntegrationEventLogService>>(
			sp => (DbConnection c) => new ImportSuccessIntegrationEventLogService(c));

		return services;
	}

	public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddTransient<IIntegrationEventHandler<UserCreatedIntegrationEvent>, UserCreatedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<UserLockoutChangedIntegrationEvent>, UserLockoutChangedIntegrationEventHandler>();

		services.AddTransient<IIntegrationEventHandler<MovingCreatedIntegrationEvent>, MovingCreatedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<MovingUpdatedIntegrationEvent>, MovingUpdatedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<MovingDeletedIntegrationEvent>, MovingDeletedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<MovingBulkDeletedIntegrationEvent>, MovingBulkDeletedIntegrationEventHandler>();

		services.AddTransient<IIntegrationEventHandler<ReceiptCreatedIntegrationEvent>, ReceiptCreatedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<ReceiptUpdatedIntegrationEvent>, ReceiptUpdatedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<ReceiptDeletedIntegrationEvent>, ReceiptDeletedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<ReceiptBulkDeletedIntegrationEvent>, ReceiptBulkDeletedIntegrationEventHandler>();

		services.AddTransient<IIntegrationEventHandler<WriteOffCreatedIntegrationEvent>, WriteOffCreatedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<WriteOffUpdatedIntegrationEvent>, WriteOffUpdatedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<WriteOffDeletedIntegrationEvent>, WriteOffDeletedIntegrationEventHandler>();
		services.AddTransient<IIntegrationEventHandler<WriteOffBulkDeletedIntegrationEvent>, WriteOffBulkDeletedIntegrationEventHandler>();

		return services;
	}

	public static async Task<IServiceProvider> InitialRabbitMqSubscribeAsync(this IServiceProvider services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		await using (var scope = services.CreateAsyncScope())
		{
			var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

			eventBus.Subscribe<UserCreatedIntegrationEvent, IIntegrationEventHandler<UserCreatedIntegrationEvent>>();
			eventBus.Subscribe<UserLockoutChangedIntegrationEvent, IIntegrationEventHandler<UserLockoutChangedIntegrationEvent>>();

			eventBus.Subscribe<MovingCreatedIntegrationEvent, IIntegrationEventHandler<MovingCreatedIntegrationEvent>>();
			eventBus.Subscribe<MovingUpdatedIntegrationEvent, IIntegrationEventHandler<MovingUpdatedIntegrationEvent>>();
			eventBus.Subscribe<MovingDeletedIntegrationEvent, IIntegrationEventHandler<MovingDeletedIntegrationEvent>>();
			eventBus.Subscribe<MovingBulkDeletedIntegrationEvent, IIntegrationEventHandler<MovingBulkDeletedIntegrationEvent>>();

			eventBus.Subscribe<ReceiptCreatedIntegrationEvent, IIntegrationEventHandler<ReceiptCreatedIntegrationEvent>>();
			eventBus.Subscribe<ReceiptUpdatedIntegrationEvent, IIntegrationEventHandler<ReceiptUpdatedIntegrationEvent>>();
			eventBus.Subscribe<ReceiptDeletedIntegrationEvent, IIntegrationEventHandler<ReceiptDeletedIntegrationEvent>>();
			eventBus.Subscribe<ReceiptBulkDeletedIntegrationEvent, IIntegrationEventHandler<ReceiptBulkDeletedIntegrationEvent>>();

			eventBus.Subscribe<WriteOffCreatedIntegrationEvent, IIntegrationEventHandler<WriteOffCreatedIntegrationEvent>>();
			eventBus.Subscribe<WriteOffUpdatedIntegrationEvent, IIntegrationEventHandler<WriteOffUpdatedIntegrationEvent>>();
			eventBus.Subscribe<WriteOffDeletedIntegrationEvent, IIntegrationEventHandler<WriteOffDeletedIntegrationEvent>>();
			eventBus.Subscribe<WriteOffBulkDeletedIntegrationEvent, IIntegrationEventHandler<WriteOffBulkDeletedIntegrationEvent>>();
		}

		return services;
	}
}
