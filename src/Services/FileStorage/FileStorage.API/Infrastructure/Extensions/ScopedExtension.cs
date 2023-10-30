using FileStorage.API.Infrastructure.Settings;
using FileStorage.API.Services;
using FileStorage.API.Services.Interfaces;

using Service.Common.Interfaces;
using Service.Common.Services;

namespace FileStorage.API.Infrastructure.Extensions;

public static class ScopedExtension
{
	public static IServiceCollection AddScopedServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddScoped<IFileStorageService, FileStorageService>()
			.AddOptions<MongoDbSettings>().BindConfiguration($"{MongoDbSettings.SectionName}");

		services
			.AddScoped<ITokenService, TokenService>();

		return services;
	}
}
