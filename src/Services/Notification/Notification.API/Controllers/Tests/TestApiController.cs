using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Notification.API.Controllers.Tests;

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

	[HttpGet("notification")]
	public async Task<IActionResult> Notification()
	{
		_logger.LogInformation("Log in Notification API");

		await Task.Delay(300);

		return Ok(new { Message = "Hello Notification!" });
	}
}
