using System.Data.Common;

using IntegrationEventLogEF;
using IntegrationEventLogEF.Services.Interfaces;

namespace Identity.API.BackgroundTask.Extensions;

public static class IntegrationExtension
{
    public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddTransient<Func<DbConnection, IExportIntegrationEventLogService>>(
            sp => (DbConnection c) => new ExportIntegrationEventLogService(c));

        return services;
    }
}
