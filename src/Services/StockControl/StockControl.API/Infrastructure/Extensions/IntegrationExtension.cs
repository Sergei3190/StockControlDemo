using System.Data.Common;

using EventBus.Interfaces;

using IntegrationEventLogEF;
using IntegrationEventLogEF.Services.Interfaces;

using Service.Common.Integration;
using Service.Common.Integration.Events.Identity;
using StockControl.API.IntegrationEvenHandlers;
using StockControl.API.Services;

namespace StockControl.API.Infrastructure.Extensions;

public static class IntegrationExtension
{
    public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        // указанные ниже сервисы нужня для сохранения интеграционного события, которое будет отправлено из данного api
        services.AddTransient<Func<DbConnection, IExportIntegrationEventLogService>>(
            sp => (DbConnection c) => new ExportIntegrationEventLogService(c));

        // указанные ниже сервисы нужня для сохранения успешности обработки интеграционного события, которое будет принято в данном api
        services.AddTransient<Func<DbConnection, IImportSuccessIntegrationEventLogService>>(
            sp => (DbConnection c) => new ImportSuccessIntegrationEventLogService(c));

		services.AddTransient<IIntegrationEventService, IdentityIntegrationEventService>();

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
