namespace Web.StockControl.HttpAggregator.Infrastructure.Extensions;

public static class ReverseProxyExtension
{
	public static IServiceCollection AddBffReverseProxy(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddReverseProxy()
			.LoadFromConfig(configuration.GetRequiredSection("ReverseProxy"));

		return services;
	}	
}
