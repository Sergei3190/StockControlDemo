using PersonalCabinet.API.DAL.Context;
using PersonalCabinet.API.Services;
using PersonalCabinet.API.Services.Interfaces;
using PersonalCabinet.API.Services.Interfaces.Select;
using PersonalCabinet.API.Services.Select;

using Service.Common.Interfaces;
using Service.Common.Services;

namespace PersonalCabinet.API.Infrastructure.Extensions;

public static class ScopedExtension
{
	public static IServiceCollection AddScopedServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services
			.AddScoped<IDbInitializerService, DbInitializerService>()
			.AddScoped<IIdentityService, IdentityService>()
			.AddScoped<ISaveService<PersonalCabinetDB>, SaveService>()
			.AddScoped<ILoadedDataTypesService, LoadedDataTypesService>()
			.AddScoped<ICacheFilesService, CacheFilesService>()
			.AddScoped<IPhotosService, PhotosService>()
			.AddScoped<IDocumentsService, DocumentsService>()
			.AddScoped<IUserPersonsService, UserPersonsService>()
			.AddScoped<IImagesService, ImagesService>();

		return services;
	}
}
