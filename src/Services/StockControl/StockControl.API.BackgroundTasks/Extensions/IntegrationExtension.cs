using IntegrationEventLogEF.Dapper;

namespace StockControl.API.BackgroundTask.Extensions;

public static class IntegrationExtension
{
	public static IServiceCollection AddIntegrationServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddTransient<Func<string, IExportIntegrationEventLogDapperService>>(
			sp => (string c) => new ExportIntegrationEventLogDapperService(c));

		return services;
	}
}
