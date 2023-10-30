using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalCabinet.API.Controllers.Tests;

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

	[HttpGet("personal-cabinet")]
	public async Task<IActionResult> PersonalCabinet()
	{
		_logger.LogInformation("Log in Personal Cabinet API");

		await Task.Delay(400);

		return Ok(new { Message = "Hello Personal Cabinet!" });
	}
}
