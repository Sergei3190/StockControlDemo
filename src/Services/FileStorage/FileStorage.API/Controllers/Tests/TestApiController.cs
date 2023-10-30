using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Service.Common.Interfaces;

namespace FileStorage.API.Controllers.Tests;

[ApiController]
[Authorize]
[Route("api/test")]
public class TestApiController : ControllerBase
{
	private readonly ILogger<TestApiController> _logger;
	private readonly ITokenService _tokenService;
	private readonly HttpClient _httpClient;

	public TestApiController(ILogger<TestApiController> logger, ITokenService tokenService, HttpClient httpClient)
	{
		_logger = logger;
		_tokenService = tokenService;
		_httpClient = httpClient;
	}

	[HttpGet("file-storage")]
	public async Task<IActionResult> FileStorage()
	{
		_logger.LogInformation("Log in File Storage API");

		await Task.Delay(200);

		return Ok(new { Message = "Hello File Storage!" });
	}
}
