namespace PersonalCabinet.FunctionalTests;

public class PersonalCabinetScenarios : PersonalCabinetScenarioBase
{
    [Fact]
    public async Task Get_test_api_should_return_http_status_code_ok()
    {
        using var server = CreateServer();

        var response = await server.CreateClient()
            .GetAsync(Get.PersonalCabinet)
            .ConfigureAwait(false);

        var responseString = await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
