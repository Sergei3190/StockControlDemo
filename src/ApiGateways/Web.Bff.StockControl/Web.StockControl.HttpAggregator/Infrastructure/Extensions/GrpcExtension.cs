using GrpcNote;

using Service.Common.Interfaces;

using Web.StockControl.HttpAggregator.Grpc.ClientServices;
using Web.StockControl.HttpAggregator.Grpc.ClientServices.Intefaces;
using Web.StockControl.HttpAggregator.Infrastructure.Settings;

namespace Web.StockControl.HttpAggregator.Infrastructure.Extensions;

public static class GrpcExtension
{
	public static IServiceCollection AddGrpcServices(this IServiceCollection services, IConfiguration configuration)
	{
		var grpcUrlsSection = configuration.GetSection("GrpcUrls");

		if (!grpcUrlsSection.Exists())
			return services;

		services.Configure<GrpcUrlsSettings>(grpcUrlsSection);

		var grpcUrlsSetting = grpcUrlsSection.Get<GrpcUrlsSettings>()!;

		services
			.AddScoped<GrpcInterceptor>()
			.AddScoped<INoteGrpcClientService, NoteGrpcClientService>();

		//создаем канал для обмена сообщениями с сервером
		services
			.AddGrpcClient<Note.NoteClient>((services, options) =>
			{
				var noteApi = grpcUrlsSetting.NoteUrl;
				options.Address = new Uri(noteApi!);
			})
			.AddCallCredentials(async (context, metadata, serviceProvider) =>
			{
				// Нужно обязательно получить токен авторизации, особенно если хотим получать данные о пользователе в сервисах grpc
				// Получив здесь токен, при обращении к IHttpContextAccessor _context внутри микросервиса, где располагается сервис grpc, мы сможем получать данные
				// о пользователе, как и при вызове метода context.GetHttpContext().User, где контекстом будет ServerCallContext.
				var service = serviceProvider.GetRequiredService<ITokenService>();
				var token = await service.GetTokenAsync();

				if (!string.IsNullOrEmpty(token))
				{
					metadata.Add("Authorization", $"Bearer {token}");
				}
			})
			// явно указываем что мы не используем TLS, рекомендации использовать http
			// https://stackoverflow.com/questions/70152113/grpc-core-rpcexception-status-statuscode-unavailable-detail-error-sta
			.ConfigureChannel(o => o.UnsafeUseInsecureChannelCallCredentials = true)
			.AddInterceptor<GrpcInterceptor>();

		return services;
	}
}
