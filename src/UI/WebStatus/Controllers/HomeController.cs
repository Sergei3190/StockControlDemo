using Microsoft.AspNetCore.Mvc;

namespace WebStatus.Controllers;

public class HomeController : Controller
{
    private IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return Redirect("/healthchecks-ui");
    }

    public IActionResult Config()
    {
        var configurationValues = _configuration.GetSection("HealthChecksUI:HealthChecks")
            .GetChildren()
            .SelectMany(cs => cs.GetChildren())
            .ToDictionary(v => v.Path, v => v.Value);

        return View(configurationValues);
    }

    public IActionResult Error()
    {
        return View();
    }
}
