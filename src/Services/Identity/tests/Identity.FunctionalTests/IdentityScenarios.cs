namespace Identity.FunctionalTests;

public class IdentityScenarios : IdentityScenarioBase
{
    [Fact]
    public async Task Get_home_page_should_return_http_status_code_ok()
    {
        using var server = CreateServer();

        var response = await server.CreateClient()
            .GetAsync(Get.Identity)
            .ConfigureAwait(false);

        var responseString = await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
