using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Service.Common.Extensions;

public static class RedisExtension
{
	public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddStackExchangeRedisCache(options =>
		{
			var cacheType = configuration["Cache:Type"];
			options.Configuration = configuration.GetConnectionString(cacheType!);
		});

		return services;
	}
}