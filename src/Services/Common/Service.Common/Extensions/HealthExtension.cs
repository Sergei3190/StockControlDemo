using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Service.Common.Integration.Settings;

namespace Service.Common.Extensions;

//https://github.com/nazmul1985/health-check-dotnet-core
public static class HealthExtension
{
	public static IHealthChecksBuilder AddDefaultHealthChecks(this IServiceCollection services, IConfiguration? configuration = null, string? dbName = null)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		var hcBuilder = services.AddHealthChecks();

		hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());

		if (configuration is null)
			return hcBuilder;

		var dbType = configuration["Db:Type"];
		var connectionString = configuration.GetConnectionString(dbType!);

		if (connectionString != null)
		{
			hcBuilder.AddSqlServer(_ =>
				connectionString,
				name: $"{dbName}-check",
				tags: new string[] { "ready" });
		}

		var cacheType = configuration["Cache:Type"];
		var redisConnection = configuration.GetConnectionString(cacheType!);

		if (!string.IsNullOrEmpty(redisConnection))
			hcBuilder.AddRedis(_ =>
				redisConnection,
				name: "redis",
				tags: new string[] { "ready" });

		var noSqlType = configuration["MongoDB:Type"];
		var mongoDbConnection = configuration.GetValue<string>($"MongoDB:{noSqlType}");

		if (!string.IsNullOrEmpty(mongoDbConnection))
			hcBuilder.AddMongoDb(_ =>
				mongoDbConnection,
				name: "mongoDB",
				tags: new string[] { "ready" });

		var eventBusSection = configuration.GetSection("EventBus");

		if (!eventBusSection.Exists() || !configuration.GetValue<bool>("EventBus:Enabled"))
			return hcBuilder;

		var eventBusSetting = eventBusSection.Get<EventBusSetting>()!;

		var amqpConnection = eventBusSetting.Connect.Equals(nameof(eventBusSetting.BusAccess.Host))
			? $"amqp://{eventBusSetting.BusAccess.Host}"
			: $"amqp://admin_docker:y2ysjnXx9vx10zT@{eventBusSetting.BusAccess.DockerHost}:5672"; // учетные данные должны совпадать с данными из .env !!!

		return hcBuilder.AddRabbitMQ(
			_ => amqpConnection,
			name: "rabbitmq",
			tags: new string[] { "ready" });
	}

	public static void MapDefaultHealthChecks(this IEndpointRouteBuilder routes)
	{
		ArgumentNullException.ThrowIfNull(routes, nameof(routes));

		// маршрут дляпроверки вспомогательных сервисов
		routes.MapHealthChecks("/hc", new HealthCheckOptions()
		{
			// будут применяться проверки только с тэгом ready
			Predicate = r => r.Tags.Contains("ready"),
			ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
		});

		routes.MapHealthChecks("/liveness", new HealthCheckOptions
		{
			Predicate = r => r.Name.Contains("self")
		});
	}
}
