using Service.Common.Extensions;

using Web.StockControl.HttpAggregator.Infrastructure.Settings;

namespace Web.StockControl.HttpAggregator.Infrastructure.Extensions;

public static class HealthExtension
{
	public static IHealthChecksBuilder AddBffHealthChecks(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		var hcBuilder = services.AddDefaultHealthChecks(configuration);

		var hcUrlsSection = configuration.GetSection("HcUrls");

		if (!hcUrlsSection.Exists())
			return hcBuilder;

		services.Configure<HcUrlsSettings>(hcUrlsSection);

		var hcUrlsSetting = hcUrlsSection.Get<HcUrlsSettings>()!;

		hcBuilder
			.AddUrlGroup(_ => new Uri(hcUrlsSetting.NoteHcUrl!), name: "note-api-check", tags: new string[] { "ready" })
			.AddUrlGroup(_ => new Uri(hcUrlsSetting.NotificationHcUrl!), name: "notification-api-check", tags: new string[] { "ready" })
			.AddUrlGroup(_ => new Uri(hcUrlsSetting.PersonalCabinetHcUrl!), name: "personal-cabinet-api-check", tags: new string[] { "ready" })
			.AddUrlGroup(_ => new Uri(hcUrlsSetting.StockControlHcUrl!), name: "stock-availability-api-check", tags: new string[] { "ready" })
			.AddUrlGroup(_ => new Uri(hcUrlsSetting.FileStorageHcUrl!), name: "file-storage-api-check", tags: new string[] { "ready" });

		return hcBuilder;
	}
}