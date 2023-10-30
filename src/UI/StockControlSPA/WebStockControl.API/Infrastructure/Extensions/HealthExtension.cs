using Service.Common.Extensions;

namespace WebStockControl.API.Infrastructure.Extensions;

public static class HealthExtension
{
    public static IHealthChecksBuilder AddSpaHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var hcBuilder = services.AddDefaultHealthChecks(configuration);

        var hcUrlSection = configuration.GetSection("HcUrls");

        if (!hcUrlSection.Exists())
            return hcBuilder;

        hcBuilder
            .AddUrlGroup(_ => new Uri(hcUrlSection.GetRequiredValue("BffHcUrl")), name: "bff-api-check", tags: new string[] { "ready" })
            .AddUrlGroup(_ => new Uri(hcUrlSection.GetRequiredValue("IdentityHcUrl")), name: "identity-api-check", tags: new string[] { "ready" });

        return hcBuilder;
    }
}
