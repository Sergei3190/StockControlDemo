using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;

using Service.Common.Extensions;

using StackExchange.Redis;

namespace Notification.API.Infrastructure.Extensions;

public static class SignalRExtension
{
	public static IServiceCollection AddNotificationSignalR(this IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services, nameof(services));

		var cacheType = configuration["Cache:Type"];
		var redisConnection = configuration.GetConnectionString(cacheType!);

		if (!string.IsNullOrEmpty(redisConnection))
		{
			// для масштабируемости SignalR
			var channelPrefix = configuration.GetValue<string>("SignalR:ChannelPrefix");

			if (channelPrefix != null)
			{
				services.AddSignalR().AddStackExchangeRedis(redisConnection, options =>
				{
					// https://stackexchange.github.io/StackExchange.Redis/Configuration.html
					options.Configuration.ChannelPrefix = RedisChannel.Pattern(channelPrefix);
				});
			}
			services.AddSignalR().AddStackExchangeRedis(redisConnection);
		}
		else
			services.AddSignalR();

		services.AddTransient<Func<IConfiguration, string>>(
			sp => (IConfiguration config) => configuration.GetRequiredValue("SignalR:Group"));

		// чтобы установить защищённое соединения с хабом
		return services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
		{
			options.Events = new JwtBearerEvents
			{
				OnMessageReceived = context =>
				{
					var accessToken = context.Request.Query["access_token"];

					var endpoint = context.HttpContext.GetEndpoint();

					// При получении сообщения убедимся, что это конечная точка Hub.
					if (endpoint?.Metadata.GetMetadata<HubMetadata>() is null)
						return Task.CompletedTask;

					context.Token = accessToken;

					return Task.CompletedTask;
				}
			};
		});
	}
}