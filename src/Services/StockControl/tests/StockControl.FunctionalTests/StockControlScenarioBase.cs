namespace StockControl.FunctionalTests;

public class StockControlScenarioBase
{
    private class StockControlApplication : WebApplicationFactory<Program>
    {
        public TestServer CreateServer()
        {
            return Server;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices((httpContext, services) =>
            {
                services.AddSingleton<IStartupFilter, AuthStartupFilter>();
            });

            builder.ConfigureAppConfiguration(c =>
            {
                var directory = Path.GetDirectoryName(typeof(StockControlScenarioBase).Assembly.Location)!;

                var fullPath = Path.Combine(directory, "appsettings.StockControl.json");

                c.AddJsonFile(fullPath, optional: false);
            });

            return base.CreateHost(builder);
        }
    }

    public TestServer CreateServer()
    {
        var factory = new StockControlApplication();
        return factory.CreateServer();
    }

    public static class Get
    {
        public static string StockControl = "api/test/stock-control";
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
