using Web.StockControl.HttpAggregator.FunctionalTests;

namespace WebStockControlHttpAggregator.FunctionalTests;

public class WebStockControlHttpAggregatorScenarios : WebStockControlHttpAggregatorScenarioBase
{
	[Fact]
	public async Task Get_test_api_should_return_http_status_code_ok()
	{
		using var server = CreateServer();

		var response = await server.CreateClient()
			.GetAsync(Get.WebStockControlHttpAggregator)
			.ConfigureAwait(false);

		var responseString = await response
			.EnsureSuccessStatusCode()
			.Content
			.ReadAsStringAsync();

		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
	}
}
