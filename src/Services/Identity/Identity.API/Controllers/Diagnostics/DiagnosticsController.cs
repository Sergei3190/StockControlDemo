using Identity.API.Infrastructure.Attributes;
using Identity.API.Models.View.Diagnostics;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers.Diagnostics;

[SecurityHeaders]
[Authorize]
public class DiagnosticsController : Controller
{
    public async Task<IActionResult> Index()
    {
        var localAddresses = new string[] { "127.0.0.1", "::1", HttpContext.Connection.LocalIpAddress.ToString() };

        // данная инфа доступна только локально
        if (!localAddresses.Contains(HttpContext.Connection.RemoteIpAddress.ToString()))
        {
            return NotFound();
        }

        var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());
        return View(model);
    }
}
