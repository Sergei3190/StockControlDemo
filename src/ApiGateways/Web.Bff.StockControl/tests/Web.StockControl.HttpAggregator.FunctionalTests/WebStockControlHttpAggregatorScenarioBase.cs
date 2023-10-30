namespace Web.StockControl.HttpAggregator.FunctionalTests;

public class WebStockControlHttpAggregatorScenarioBase
{
    private class WebStockControlHttpAggregatorApplication : WebApplicationFactory<Program>
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
        var factory = new WebStockControlHttpAggregatorApplication();
        return factory.CreateServer();
    }

    public static class Get
    {
        public static string WebStockControlHttpAggregator = "api/test/web-sk-bff";
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
