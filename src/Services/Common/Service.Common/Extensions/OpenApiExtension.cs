using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

using Service.Common.OperationFilter;

namespace Service.Common.Extensions;

/// <summary>
/// Расширение для взаимодействия swagger с системой identity
/// </summary>
public static class OpenApiExtension
{
	public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		var openApi = configuration.GetSection("OpenApi");

		if (!openApi.Exists())
			return services;

		return services.AddSwaggerGen(options =>
		{
			IdentityModelEventSource.ShowPII = true;

			var document = openApi.GetRequiredSection("Document");
			var versionFromConfig = document.GetValue<string>("Version");
			var version = string.IsNullOrEmpty(versionFromConfig) ? "v1" : versionFromConfig;

			options.SwaggerDoc(version, new OpenApiInfo
			{
				Title = document.GetRequiredValue("Title"),
				Version = version,
				Description = document.GetRequiredValue("Description")
			});

			var identitySection = configuration.GetSection("Identity");

			if (!identitySection.Exists())
				return;

			var identityUrlExternal = identitySection["ExternalUrl"] ?? identitySection.GetRequiredValue("Url");

			// ключом является имя scope в Congig.cs Identity.API
			var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);

			options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.OAuth2,
				Flows = new OpenApiOAuthFlows()
				{
					Implicit = new OpenApiOAuthFlow()
					{
						AuthorizationUrl = new Uri($"{identityUrlExternal}/connect/authorize"),
						TokenUrl = new Uri($"{identityUrlExternal}/connect/token"),
						Scopes = scopes,
					}
				}
			});

			options.OperationFilter<AuthorizeCheckOperationFilter>();
		});
	}

	public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(app, nameof(app));

		var openApiSection = configuration.GetSection("OpenApi");

		if (!openApiSection.Exists())
			return app;

		// нужно установить нугет Swashbuckle.AspNetCore
		app.UseSwagger();

		// с помощью данной настройки можем перейти по созданному урлу и получить файл json с конечными точками и компоненами схем безопасности
		app.UseSwaggerUI(setup =>
		{
			var authSection = openApiSection.GetSection("Auth");
			var endpointSection = openApiSection.GetRequiredSection("Endpoint");
			var versionFromConfig = openApiSection.GetValue<string>("Document:Version");
			var version = string.IsNullOrEmpty(versionFromConfig) ? "v1" : versionFromConfig;

			var swaggerUrl = $"/swagger/{version}/swagger.json";

			setup.SwaggerEndpoint(swaggerUrl, endpointSection.GetRequiredValue("Name"));

			if (authSection.Exists())
			{
				setup.OAuthClientId(authSection.GetRequiredValue("ClientId"));
				setup.OAuthAppName(authSection.GetRequiredValue("AppName"));

				var identitySection = configuration.GetSection("Identity");

				if (identitySection.Exists())
				{
					var scopes = identitySection.GetSection("scopes")
						.GetChildren()
						.Select(x => x.Key)
						.ToArray();

					setup.OAuthScopes(scopes);
				}
			}
		});

		app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

		return app;
	}
}