using System.IdentityModel.Tokens.Jwt;

using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace Service.Common.Extensions;

public static class AuthenticationExtension
{
	public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		var identitySection = configuration.GetSection("Identity");

		if (!identitySection.Exists())
			return services;

		// предотвратить сопоставление утверждения "sub" с идентификатором имени.
		JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

		services.AddAuthentication()
			.AddJwtBearer(options =>
			{
				var identityUrl = identitySection.GetRequiredValue("Url");
				var audience = identitySection.GetRequiredValue("Audience");

				options.Authority = identityUrl;

				// Получает или задает значение, указывающее, требуется ли протокол HTTPS для адреса или центра метаданных.
				// Значение по умолчанию — true. Это должно быть отключено только в средах разработки.
				options.RequireHttpsMetadata = false;
				options.Audience = audience;
				options.TokenValidationParameters.ValidateAudience = false;
			});

		return services;
	}
}