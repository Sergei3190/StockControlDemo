using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StockControl.API.Controllers.Tests;

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

	[HttpGet("stock-control")]
	public async Task<IActionResult> StockControl()
	{
		_logger.LogInformation("Log in Stock Control API");

		await Task.Delay(200);

		return Ok(new { Message = "Hello Stock Control!" });
	}
}
