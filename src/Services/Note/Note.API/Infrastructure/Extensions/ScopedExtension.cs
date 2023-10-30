using Note.API.DAL.Context;
using Note.API.Services;
using Note.API.Services.Interfaces;

using Service.Common.Interfaces;
using Service.Common.Services;

namespace Note.API.Infrastructure.Extensions;

public static class ScopedExtension
{
	public static IServiceCollection AddScopedServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services
			.AddScoped<IDbInitializerService, DbInitializerService>()
			.AddScoped<IIdentityService, IdentityService>()
			.AddScoped<ISaveService<NoteDB>, SaveService>()
			.AddScoped<INotesService, NotesService>();

		return services;
	}
}
