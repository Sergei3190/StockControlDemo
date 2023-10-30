using System.Security.Claims;

using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;

using Identity.API.Domain.Entities;
using Identity.API.Infrastructure.Attributes;
using Identity.API.Infrastructure.Extensions;

using IdentityModel;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers.Account;

[SecurityHeaders]
[AllowAnonymous]
public class ExternalController : Controller
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly IIdentityServerInteractionService _interaction;
	private readonly IClientStore _clientStore;
	private readonly IEventService _events;
	private readonly ILogger<ExternalController> _logger;

	public ExternalController(
		UserManager<User> userManager,
		SignInManager<User> signInManager,
		IIdentityServerInteractionService interaction,
		IClientStore clientStore,
		IEventService events,
		ILogger<ExternalController> logger)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_interaction = interaction;
		_clientStore = clientStore;
		_events = events;
		_logger = logger;
	}

	/// <summary>
	/// Инициировать обходной путь к внешнему провайдеру аутентификации, вызывается из GET account/login
	/// </summary>
	[HttpGet]
	public IActionResult Challenge(string scheme, string returnUrl)
	{
		if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

		if (Url.IsLocalUrl(returnUrl) == false && _interaction.IsValidReturnUrl(returnUrl) == false)
			throw new Exception("invalid return URL");

		var props = new AuthenticationProperties
		{
			RedirectUri = Url.Action(nameof(Callback)),
			Items =
				{
					{ "returnUrl", returnUrl },
					{ "scheme", scheme },
				}
		};

		return Challenge(props, scheme);
	}

	/// <summary>
	/// Обработка внешней аутентификации
	/// </summary>
	[HttpGet]
	public async Task<IActionResult> Callback()
	{
		var result = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

		if (result?.Succeeded != true)
			throw new Exception("External authentication error");

		if (_logger.IsEnabled(LogLevel.Debug))
		{
			var externalClaims = result.Principal.Claims.Select(c => $"{c.Type}: {c.Value}");
			_logger.LogDebug("External claims: {@claims}", externalClaims);
		}

		var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);

		if (user == null)
		{
			user = await AutoProvisionUserAsync(provider, providerUserId, claims);
		}

		var additionalLocalClaims = new List<Claim>();
		var localSignInProps = new AuthenticationProperties();
		ProcessLoginCallback(result, additionalLocalClaims, localSignInProps);

		var principal = await _signInManager.CreateUserPrincipalAsync(user);
		additionalLocalClaims.AddRange(principal.Claims);
		var name = principal.FindFirst(JwtClaimTypes.Name)?.Value ?? user.Id.ToString();

		var isuser = new IdentityServerUser(user.Id.ToString())
		{
			DisplayName = name,
			IdentityProvider = provider,
			AdditionalClaims = additionalLocalClaims
		};

		await HttpContext.SignInAsync(isuser, localSignInProps);

		await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

		var returnUrl = result.Properties.Items["returnUrl"] ?? "~/";

		var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
		await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id.ToString(), name, true, context?.Client.ClientId));

		if (context != null)
		{
			if (context.IsNativeClient())
				return this.LoadingPage("Redirect", returnUrl);
		}

		return Redirect(returnUrl);
	}

	private async Task<(User user, string provider, string providerUserId, IEnumerable<Claim> claims)>
		FindUserFromExternalProviderAsync(AuthenticateResult result)
	{
		var externalUser = result.Principal;

		var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
						  externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
						  throw new Exception("Unknown userid");

		var claims = externalUser.Claims.ToList();
		claims.Remove(userIdClaim);

		var provider = result.Properties.Items["scheme"];
		var providerUserId = userIdClaim.Value;

		var user = await _userManager.FindByLoginAsync(provider, providerUserId);

		return (user, provider, providerUserId, claims);
	}

	/// <summary>
	/// Создаём пользователя на основании предоставленных внешним провайдером разрешений
	/// </summary>
	/// <param name="provider"></param>
	/// <param name="providerUserId"></param>
	/// <param name="claims"></param>
	/// <returns></returns>
	/// <exception cref="Exception"></exception>
	private async Task<User> AutoProvisionUserAsync(string provider, string providerUserId, IEnumerable<Claim> claims)
	{
		var filtered = new List<Claim>();

		var name = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
			claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

		if (name != null)
		{
			filtered.Add(new Claim(JwtClaimTypes.Name, name));
		}
		else
		{
			var first = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
				claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
			var last = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
				claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
			if (first != null && last != null)
			{
				filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
			}
			else if (first != null)
			{
				filtered.Add(new Claim(JwtClaimTypes.Name, first));
			}
			else if (last != null)
			{
				filtered.Add(new Claim(JwtClaimTypes.Name, last));
			}
		}

		var email = claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
		   claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
		if (email != null)
		{
			filtered.Add(new Claim(JwtClaimTypes.Email, email));
		}

		var user = new User
		{
			UserName = Guid.NewGuid().ToString(),
		};
		var identityResult = await _userManager.CreateAsync(user);
		if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

		if (filtered.Any())
		{
			identityResult = await _userManager.AddClaimsAsync(user, filtered);
			if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
		}

		identityResult = await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
		if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);

		return user;
	}

	// если внешний логин основан на OIDC, есть определенные вещи, которые мы должны сохранить, чтобы выход из системы работал
	// это будет отличаться для WS-Fed, SAML2p или других протоколов
	private void ProcessLoginCallback(AuthenticateResult externalResult, List<Claim> localClaims, AuthenticationProperties localSignInProps)
	{
		// если внешняя система отправила запрос идентификатора сессии, скопируйте его.
		// чтобы мы могли использовать его для однократного выхода из системы
		var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
		if (sid != null)
		{
			localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
		}

		// если внешний провайдер выдал id_token, мы сохраним его для выхода из системы
		var idToken = externalResult.Properties.GetTokenValue("id_token");
		if (idToken != null)
		{
			localSignInProps.StoreTokens(new[] { new AuthenticationToken { Name = "id_token", Value = idToken } });
		}
	}
}
