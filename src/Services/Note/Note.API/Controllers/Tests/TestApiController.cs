using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Note.API.Controllers.Tests;

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

	[HttpGet("note")]
	public async Task<IActionResult> Note()
	{
		_logger.LogInformation("Log in Note API");

		await Task.Delay(200);

		return Ok(new { Message = "Hello Note!" });
	}
}
