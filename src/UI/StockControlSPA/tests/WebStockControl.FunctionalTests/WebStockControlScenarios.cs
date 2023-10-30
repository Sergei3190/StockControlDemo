namespace WebStockControl.FunctionalTests;

public class WebStockControlScenarios : WebStockControlScenarioBase
{
    [Fact]
    public async Task Get_config_should_return_http_status_code_ok()
    {
        using var server = CreateServer();

        var response = await server.CreateClient()
            .GetAsync(Get.ConfigGet)
            .ConfigureAwait(false);

        var responseString = await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
