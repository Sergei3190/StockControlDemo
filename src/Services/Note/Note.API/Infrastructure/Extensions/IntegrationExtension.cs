using System.Data.Common;

using EventBus.Interfaces;

using IntegrationEventLogEF;

using Note.API.IntegrationEvenHandlers;
using Service.Common.Integration.Events.Identity;

namespace Note.API.Infrastructure.Extensions;

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
        }

        return services;
    }
}
