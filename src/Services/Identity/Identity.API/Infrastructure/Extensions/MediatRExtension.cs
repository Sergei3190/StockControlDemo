using Identity.API.MediatR.Behaviors;

namespace Identity.API.Infrastructure.Extensions;

public static class MediatRExtension
{
	public static IServiceCollection AddIdentityMediatR(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		services.AddMediatR(cfg =>
		{
			cfg.RegisterServicesFromAssemblyContaining(typeof(Program));
			cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
			cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
		});

		return services;
	}
}
