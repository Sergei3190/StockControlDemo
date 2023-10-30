namespace Note.FunctionalTests;

public class NoteScenarioBase
{
    private class NoteApplication : WebApplicationFactory<Program>
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

            builder.ConfigureAppConfiguration(c =>
            {
                var directory = Path.GetDirectoryName(typeof(NoteScenarioBase).Assembly.Location)!;

                var fullPath = Path.Combine(directory, "appsettings.Note.json");

                c.AddJsonFile(fullPath, optional: false);
            });

            return base.CreateHost(builder);
        }
    }

    public TestServer CreateServer()
    {
        var factory = new NoteApplication();
        return factory.CreateServer();
    }

    public static class Get
    {
        public static string Note = "api/test/note";
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
