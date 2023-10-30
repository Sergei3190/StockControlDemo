using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Service.Common.Configs;
using Service.Common.Settings;

namespace Service.Common.Extensions;

public static class CorsExtension
{
	public static IServiceCollection AddDefaultCors(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		// данные настройки юудут доступны только здесь
		var settings = Config.GetConfiguration().GetRequiredSection("CorsPolicy").Get<CorsSettings>()!;

		services.AddCors(options =>
		{
			options.AddPolicy(settings.Name!,
				 config =>
				 {
					 var origins = settings.AllowedOriginsList
						.Select(o => new Regex(o))
						.ToArray();

					 // Origin и AllowCredentials будут работать вместе только так
					 config.SetIsOriginAllowed(h => origins?.Any(o => o.IsMatch(h)) ?? false);
					 config.WithMethods(settings.AllowedMethodsList);
					 config.WithHeaders(settings.AllowedHeadersList);
					 config.AllowCredentials();
					 config.WithExposedHeaders(settings.ExposedHeaders!);
				 });
		});

		return services;
	}

	public static IApplicationBuilder UseDefaultCors(this IApplicationBuilder app)
	{
		ArgumentNullException.ThrowIfNull(app, nameof(app));

		app.UseCors(Config.Configuration.GetRequiredValue("CorsPolicy:Name"));

		return app;
	}
}
