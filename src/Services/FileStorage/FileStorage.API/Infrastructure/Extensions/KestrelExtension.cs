using FileStorage.API.Infrastructure.Settings;

namespace FileStorage.API.Infrastructure.Extensions;

public static class KestrelExtension
{
	public static IWebHostBuilder SetConfigureKestrel(this IWebHostBuilder builder)
	{
		ArgumentNullException.ThrowIfNull(builder, nameof(builder));

		KestrelLimitSettings kestrelLimitSettings = null!;

		builder.ConfigureServices((context, services) =>
		{
			var section = context.Configuration.GetRequiredSection(KestrelLimitSettings.SectionName);

			if (section is null)
				throw new ArgumentNullException(nameof(section), $"Не задана секция конфигурации {KestrelLimitSettings.SectionName}");

			services.Configure<KestrelLimitSettings>(section);

			kestrelLimitSettings = section.Get<KestrelLimitSettings>()!;
		});

		builder.ConfigureKestrel((context, options) =>
		{
			// увеличиваем лимит ожидания передачи данных и размер пакета, чтобы можно было передавать "большие" файлы
			// настройки будут захардкодены
			options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(kestrelLimitSettings.KeepAliveTimeout);
			options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(kestrelLimitSettings.RequestHeadersTimeout);
			options.Limits.MaxRequestBodySize = KestrelLimitSettings.MaxRequestBodySize;
		});

		return builder;
	}
}
