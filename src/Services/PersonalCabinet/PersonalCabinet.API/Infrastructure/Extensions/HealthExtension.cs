using Service.Common.Extensions;

namespace PersonalCabinet.API.Infrastructure.Extensions;

public static class HealthExtension
{
	public static IHealthChecksBuilder AddPersonalCabinetHealthChecks(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		var hcBuilder = services.AddDefaultHealthChecks(configuration, "PersonalCabinetDB");

		return hcBuilder;
	}
}
