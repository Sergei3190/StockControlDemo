namespace WebStockControl.FunctionalTests;

public class WebStockControlScenarioBase
{
    private class WebStockControlApplication : WebApplicationFactory<Program>
    {
        public TestServer CreateServer()
        {
            return Server;
        }
    }

    public TestServer CreateServer()
    {
        var factory = new WebStockControlApplication();
        return factory.CreateServer();
    }

    public static class Get
    {
        public static string ConfigGet = "api/config/get";
    }
}
