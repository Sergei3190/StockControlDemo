using FileStorage.API.MediatR.Behaviors;

namespace FileStorage.API.Infrastructure.Extensions;

public static class MediatRExtension
{
	public static IServiceCollection AddFileStorageMediatR(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
			cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
		});

		return services;
	}
}
