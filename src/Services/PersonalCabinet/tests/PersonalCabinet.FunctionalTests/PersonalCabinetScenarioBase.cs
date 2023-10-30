namespace PersonalCabinet.FunctionalTests;

public class PersonalCabinetScenarioBase
{
    private class PersonalCabinetApplication : WebApplicationFactory<Program>
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
                var directory = Path.GetDirectoryName(typeof(PersonalCabinetScenarioBase).Assembly.Location)!;

                var fullPath = Path.Combine(directory, "appsettings.PersonalCabinet.json");

                c.AddJsonFile(fullPath, optional: false);
            });

            return base.CreateHost(builder);
        }
    }

    public TestServer CreateServer()
    {
        var factory = new PersonalCabinetApplication();
        return factory.CreateServer();
    }

    public static class Get
    {
        public static string PersonalCabinet = "api/test/personal-cabinet";
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
