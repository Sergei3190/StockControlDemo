using Service.Common.DelegatingHandlers;
using Service.Common.Interfaces;
using Service.Common.Services;

namespace Web.StockControl.HttpAggregator.Infrastructure.Extensions;

public static class TransientExtension
{
	public static IServiceCollection AddTransientServices(this IServiceCollection services)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		#region проверка asses_token для мобильного и MVC клиентов

		services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
		services.AddTransient<ITokenService, TokenService>();

		#endregion

		return services;
	}
}
