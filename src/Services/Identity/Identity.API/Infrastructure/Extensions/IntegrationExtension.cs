using System.Data.Common;

using Identity.API.Services;

using IntegrationEventLogEF;
using IntegrationEventLogEF.Services.Interfaces;

using Service.Common.Integration;

namespace Identity.API.Infrastructure.Extensions;

public static class IntegrationExtension
{
    public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddTransient<Func<DbConnection, IExportIntegrationEventLogService>>(
            sp => (DbConnection c) => new ExportIntegrationEventLogService(c));

        services.AddTransient<IIntegrationEventService, IdentityIntegrationEventService>();

        return services;
    }
}
