using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.StockControl.HttpAggregator.Controllers.Tests;

[ApiController]
[Authorize]
[Route("api/test")]
public class TestApiController : ControllerBase
{
	private readonly ILogger<TestApiController> _logger;

	public TestApiController(ILogger<TestApiController> logger)
	{
		_logger = logger;
	}

	[HttpGet("web-sk-bff")]
	public async Task<IActionResult> WebStockControlHttpAggregator()
	{
		_logger.LogInformation("Log in Web.StockControl.HttpAggregator API");

		await Task.Delay(200);

		return Ok(new { Message = "Hello Web.StockControl.HttpAggregator!" });
	}
}
