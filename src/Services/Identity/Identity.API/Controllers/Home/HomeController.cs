using Duende.IdentityServer.Services;

using Identity.API.Infrastructure.Attributes;
using Identity.API.Models.View.Home;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers.Home;

[SecurityHeaders]
[AllowAnonymous]
public class HomeController : Controller
{
	private readonly IIdentityServerInteractionService _interaction;
	private readonly ILogger<HomeController> _logger;
	private readonly IWebHostEnvironment _environment;

	public HomeController(
		IIdentityServerInteractionService interaction,
		ILogger<HomeController> logger,
		IWebHostEnvironment environment)
	{
		_interaction = interaction;
		_logger = logger;
		_environment = environment;
	}

	public IActionResult Index()
	{
		if (_environment.IsDevelopment())
			return View();

		_logger.LogInformation("Homepage is disabled in production. Returning 404.");

		return NotFound();
	}

	public async Task<IActionResult> Error(string errorId)
	{
		var vm = new ErrorViewModel();

		var message = await _interaction.GetErrorContextAsync(errorId);

		if (message != null)
		{
			vm.Error = message;

			if (!_environment.IsDevelopment())
			{
				message.ErrorDescription = null;
			}
		}

		return View("Error", vm);
	}
}