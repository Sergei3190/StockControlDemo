using FileParser.Service;

using Notification.API.DAL.Context;
using Notification.API.Services;
using Notification.API.Services.Interfaces;

using Service.Common.Interfaces;
using Service.Common.Services;

namespace Notification.API.Infrastructure.Extensions;

public static class ScopedExtension
{
	public static IServiceCollection AddScopedServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services
			.AddScoped<IFileParserService, FileParserService>()
			.AddScoped<INotificationSettingsService, NotificationSettingsService>()
			.AddScoped<INotificationTypesService, NotificationTypesService>()
			.AddScoped<IIdentityService, IdentityService>()
			.AddScoped<ISaveService<NotificationDB>, SaveService>()
			.AddScoped<IDbInitializerService, DbInitializerService>()
			.AddScoped<IDbInitializerService, DbInitializerService>();

		return services;
	}
}
