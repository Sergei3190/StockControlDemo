namespace Notification.FunctionalTests;

public class NotificationScenarioBase
{
    private class NotificationApplication : WebApplicationFactory<Program>
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
                var directory = Path.GetDirectoryName(typeof(NotificationScenarioBase).Assembly.Location)!;

                var fullPath = Path.Combine(directory, "appsettings.Notification.json");

                c.AddJsonFile(fullPath, optional: false);
            });

            return base.CreateHost(builder);
        }
    }

    public TestServer CreateServer()
    {
        var factory = new NotificationApplication();
        return factory.CreateServer();
    }

    public static class Get
    {
        public static string Notification = "api/test/notification";
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
