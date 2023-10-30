using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Service.Common.OperationFilter;

internal class AuthorizeCheckOperationFilter : IOperationFilter
{
    private readonly IConfiguration _configuration;

    public AuthorizeCheckOperationFilter(IConfiguration configuration)
    {
        _configuration = configuration;
    }

	public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // проверяем наличие атрибута авторизации
        var hasAuthorize = context.MethodInfo.DeclaringType!.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

        if (!hasAuthorize)
            return;

        operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

		// создаём схему безопасности, указывая в качестве Id выбранный в options.AddSecurityDefinition протокол защиты
		var oAuthScheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
        };

        var identitySection = _configuration.GetSection("Identity");
        var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().Select(r => r.Key).ToArray();

		// созадаём механизм безопасности (он должен быть один) для операций с атрибутом авторизации для объявленных в конфигурации scope
		// ключом будет oAuthScheme, а его значением список scope
		operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                [ oAuthScheme ] = scopes
            }
        };
    }
}
