using Duende.IdentityServer.Services;

using Identity.API.Infrastructure.Loggings;
using Identity.API.Services;

using Service.Common.Interfaces;

namespace Identity.API.Infrastructure.Extensions;

public static class ScopedExtension
{
	public static IServiceCollection AddScopedServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services
			.AddScoped<IDbInitializerService, DbInitializerService>()
			.AddScoped<IEventSink, SeqEventSink>();

		return services;
	}
}
