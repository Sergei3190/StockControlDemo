namespace Identity.FunctionalTests;

public class IdentityScenarioBase
{
    private class IdentityApplication : WebApplicationFactory<Program>
    {
        public TestServer CreateServer()
        {
            return Server;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(c =>
            {
                var directory = Path.GetDirectoryName(typeof(IdentityScenarioBase).Assembly.Location)!;

                var fullPath = Path.Combine(directory, "appsettings.Identity.json");

                c.AddJsonFile(fullPath, optional: false);
            });

            return base.CreateHost(builder);
        }
    }

    public TestServer CreateServer()
    {
        var factory = new IdentityApplication();
        return factory.CreateServer();
    }

    public static class Get
    {
        public static string Identity = "/";
    }
}
