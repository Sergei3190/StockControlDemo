using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using WebStockControl.API.Infrastructure.Settings;

namespace WebStockControl.API.Controllers;

[ApiController]
[Route("api/config")]
public class ConfigApiController : ControllerBase
{
    private readonly IOptionsSnapshot<AppSettings> _settings;
    private readonly ILogger<ConfigApiController> _logger;

    public ConfigApiController(
        IOptionsSnapshot<AppSettings> options,
        ILogger<ConfigApiController> logger)
    {
        _settings = options;
        _logger = logger;
    }

    [HttpGet("get")]
    public IActionResult Get()
    {
        var config = _settings.Value;
		_logger.LogInformation("spa api {config}", config);
        return Ok(config);
    }
}