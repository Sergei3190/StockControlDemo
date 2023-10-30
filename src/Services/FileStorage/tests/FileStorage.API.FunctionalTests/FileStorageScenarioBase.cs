namespace FileStorage.API.FunctionalTests;

public class FileStorageScenarioBase
{
	private class WebStockControlApplication : WebApplicationFactory<Program>
	{
		public TestServer CreateServer()
		{
			return Server;
		}

		protected override IHost CreateHost(IHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				services.AddSingleton<IStartupFilter, AuthStartupFilter>();
			});

			return base.CreateHost(builder);
		}
	}

	public TestServer CreateServer()
	{
		var factory = new WebStockControlApplication();
		return factory.CreateServer();
	}

	public static class Get
	{
		public static string FileStorage = "api/test/file-storage";
	}

	private class AuthStartupFilter : IStartupFilter
	{
		public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
		{
			return app =>
			{
				app.UseMiddleware<AutoAuthorizeMiddleware>();

				next(app);
			};
		}
	}
}
