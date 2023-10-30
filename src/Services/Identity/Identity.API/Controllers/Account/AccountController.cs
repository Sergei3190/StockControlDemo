using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Stores;

using Email.Service;

using Identity.API.Domain.Entities;
using Identity.API.Domain.Events;
using Identity.API.Infrastructure.Attributes;
using Identity.API.Infrastructure.Events;
using Identity.API.Infrastructure.Extensions;
using Identity.API.Infrastructure.Options.Account;
using Identity.API.Infrastructure.Settings.Authentication;
using Identity.API.MediatR.Commands.Login;
using Identity.API.MediatR.Commands.Register;
using Identity.API.MediatR.Commands.ResetPassword;
using Identity.API.Models;
using Identity.API.Models.DTO;
using Identity.API.Models.Input.Account;
using Identity.API.Models.View.Account;

using IdentityModel;

using MediatR;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Identity.API.Controllers.Account;

[SecurityHeaders]
[AllowAnonymous]
public class AccountController : Controller
{
	private readonly UserManager<User> _userManager;
	private readonly SignInManager<User> _signInManager;
	private readonly IIdentityServerInteractionService _interaction;
	private readonly IClientStore _clientStore;
	private readonly IAuthenticationSchemeProvider _schemeProvider;
	private readonly IAuthenticationHandlerProvider _handlerProvider;
	private readonly IEventService _events;
	private readonly IEmailSender _emailSender;
	private readonly IMediator _mediator;
	private readonly AuthenticationSettings _authenticationSettings;

	public AccountController(
		UserManager<User> userManager,
		SignInManager<User> signInManager,
		IIdentityServerInteractionService interaction,
		IClientStore clientStore,
		IAuthenticationSchemeProvider schemeProvider,
		IAuthenticationHandlerProvider handlerProvider,
		IEventService events,
		IEmailSender emailSender,
		IMediator mediator,
		IOptionsSnapshot<AuthenticationSettings> options)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_interaction = interaction;
		_clientStore = clientStore;
		_schemeProvider = schemeProvider;
		_handlerProvider = handlerProvider;
		_events = events;
		_emailSender = emailSender;
		_mediator = mediator;
		_authenticationSettings = options.Value;
	}

	[HttpGet]
	public async Task<IActionResult> Register(string returnUrl = null)
	{
		var vm = await BuildRegisterViewModelAsync(returnUrl);

		ViewData["ReturnUrl"] = returnUrl;

		return View(vm);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Register(RegisterInputModel model, string button)
	{
		var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

		if (button != "register")
			return RedirectToAction(nameof(Login), new { model.ReturnUrl });

		if (ModelState.IsValid)
		{
			var dto = new RegisterDto(model, new(nameof(ConfirmEmail), "Account", Url, context?.Client?.ClientId, Request.Scheme));

			var creation_result = await _mediator.Send(new RegisterCommand(dto));

			if (creation_result.Succeeded)
			{
				return RedirectToAction(nameof(SuccessRegistration));
			}

			foreach (var error in creation_result.Errors)
				ModelState.AddModelError("", error.Description);

			var errorInfo = string.Join(", ", creation_result.Errors.Select(e => e.Description));
			await _events.RaiseAsync(new UserRegisterFailureEvent(model.UserName, errorInfo, clientId: context?.Client.ClientId));
		}

		var vm = await BuildRegisterViewModelAsync(model);

		ViewData["ReturnUrl"] = model.ReturnUrl;

		return View(vm);
	}

	[HttpGet]
	public async Task<IActionResult> ConfirmEmail(string token, string email, string client)
	{
		var user = await _userManager.FindByEmailAsync(email);

		if (user is null)
			return RedirectToAction("Error", "Home");

		var result = await _userManager.ConfirmEmailAsync(user, token);

		if (!result.Succeeded)
			return RedirectToAction("Error", "Home");

		await _events.RaiseAsync(new UserRegisterSuccessEvent(user.UserName, clientId: client, JsonSerializer.Serialize(new[] { Role.Users })));

		return View();
	}

	[HttpGet]
	public IActionResult SuccessRegistration()
	{
		return View();
	}

	/// <summary>
	/// Точка входа в рабочий процесс входа в систему
	/// </summary>
	[HttpGet]
	public async Task<IActionResult> Login(string returnUrl = null)
	{
		var vm = await BuildLoginViewModelAsync(returnUrl);

		ViewData["ReturnUrl"] = returnUrl;

		if (vm.IsExternalLoginOnly)
		{
			// TODO
			// у нас есть только один вариант для входа в систему, и это внешний провайдер
			return RedirectToAction("Challenge", "External", new { scheme = vm.ExternalLoginScheme, returnUrl });
		}

		return View(vm);
	}

	/// <summary>
	/// Обработка введённых пользователем данных для входа в систему
	/// </summary>
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginInputModel model, string button)
	{
		var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);

		if (button != "login")
			return await GetCancelRedirect(context, model.ReturnUrl);

		if (ModelState.IsValid)
		{
			var result = await _mediator.Send(new LoginCommand(new LoginDto(model)));

			if (result.Succeeded)
				return await HandleSuccessfulLoginAsync(model.UserName, model.ReturnUrl, context);

			await HandleUnsuccessfulLoginAsync(model.UserName, result.IsLockedOut, result.IsNotAllowed, context?.Client?.ClientId, model.ReturnUrl);
		}

		var vm = await BuildLoginViewModelAsync(model);

		ViewData["ReturnUrl"] = model.ReturnUrl;

		return View(vm);
	}

	/// <summary>
	/// Показать страницу выхода из системы
	/// </summary>
	[HttpGet]
	public async Task<IActionResult> Logout(string logoutId)
	{
		var vm = await BuildLogoutViewModelAsync(logoutId);

		if (vm.ShowLogoutPrompt == false)
		{
			return await Logout(vm);
		}

		return View(vm);
	}

	/// <summary>
	/// Обработка возврата страницы выхода из системы
	/// </summary>
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Logout(LogoutInputModel model)
	{
		var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);

		if (User?.Identity.IsAuthenticated == true)
		{
			await _signInManager.SignOutAsync();
			await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
		}

		if (vm.TriggerExternalSignout)
		{
			string url = Url.Action("Logout", new { logoutId = vm.LogoutId });

			return SignOut(new AuthenticationProperties { RedirectUri = url }, vm.ExternalAuthenticationScheme);
		}

		return View("LoggedOut", vm);
	}

	[HttpGet]
	public IActionResult AccessDenied()
	{
		return View();
	}

	/// <summary>
	/// Получение формы заполненя электронной почты, в случаи, если пользователь забыл пароль
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public IActionResult ForgotPassword(string returnUrl = null)
	{
		return View(new ForgotPasswordViewModel() { ReturnUrl = returnUrl });
	}

	/// <summary>
	/// Обработка данных пользователя для отправки токена сброса пароля на почту 
	/// </summary>
	/// <param name="model"></param>
	/// <returns></returns>
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ForgotPassword(ForgotPasswordInputModel model, string button)
	{
		if (button != "forgot")
			return RedirectToAction(nameof(Login), new { model.ReturnUrl });

		if (!ModelState.IsValid)
			return View(model);

		var client = await GetClient64Async(model.ReturnUrl);

		var user = await _userManager.FindByEmailAsync(model.Email);

		if (user is null)
			return RedirectToAction(nameof(ForgotPasswordConfirmation), new { model.ReturnUrl });

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);

		var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email, client }, Request.Scheme);

		var message = new Message(new (string name, string address)[] { (user.UserName, user.Email) }, "Сброс пароля", callback, null);

		_emailSender.SendEmailAsync(message);

		return RedirectToAction(nameof(ForgotPasswordConfirmation), new { model.ReturnUrl });
	}

	/// <summary>
	/// Возвращает подтверждение отправки токена на почту
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public IActionResult ForgotPasswordConfirmation(string returnUrl = null)
	{
		ViewBag.ReturnUrl = returnUrl;

		return View();
	}

	/// <summary>
	/// Принимает запрос из электронной почты, извлечет значения токена и электронной почты и создаст представление
	/// </summary>
	/// <returns></returns>
	[HttpGet]
	public IActionResult ResetPassword(string token, string email, string client = null)
	{
		var model = new ResetPasswordViewModel { Token = token, Email = email, Client = client };
		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> ResetPassword(ResetPasswordInputModel model)
	{
		if (!ModelState.IsValid)
			return View(model);

		var user = await _userManager.FindByEmailAsync(model.Email);

		if (user == null)
			RedirectToAction(nameof(ResetPasswordConfirmation));

		var resetPassResult = await _mediator.Send(new ResetPasswordCommand(new ResetPasswordDto(user.UserName, model.Token, model.Password)));

		if (!resetPassResult.Succeeded)
		{
			foreach (var error in resetPassResult.Errors)
			{
				ModelState.TryAddModelError(error.Code, error.Description);
			}
			return View();
		}

		return RedirectToAction(nameof(ResetPasswordConfirmation), new { model.Client });
	}

	[HttpGet]
	public IActionResult ResetPasswordConfirmation(string client = null)
	{
		var clientDe64 = GetClient(client);

		ViewBag.Client = clientDe64;

		return View();
	}

	private async Task<RegisterViewModel> BuildRegisterViewModelAsync(string returnUrl)
	{
		var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

		var allowLocal = true;

		if (context?.Client.ClientId != null)
		{
			var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);

			if (client != null)
				allowLocal = client.EnableLocalLogin;
		}

		return new RegisterViewModel
		{
			AllowRememberLogin = AccountOptions.AllowRememberLogin,
			EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
			ReturnUrl = returnUrl,
			UserName = context?.LoginHint,
			Email = context?.LoginHint,
		};
	}

	private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
	{
		var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

		if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
		{
			var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

			var vm = new LoginViewModel
			{
				EnableLocalLogin = local,
				ReturnUrl = returnUrl,
				UserName = context?.LoginHint,
			};

			if (!local)
			{
				vm.ExternalProviders = new[] { new ExternalProvider { AuthenticationScheme = context.IdP } };
			}

			return vm;
		}

		var schemes = await _schemeProvider.GetAllSchemesAsync();

		var providers = schemes
			.Where(x => x.DisplayName != null)
			.Select(x => new ExternalProvider
			{
				DisplayName = x.DisplayName ?? x.Name,
				AuthenticationScheme = x.Name
			}).ToList();

		var allowLocal = true;
		if (context?.Client.ClientId != null)
		{
			var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
			if (client != null)
			{
				allowLocal = client.EnableLocalLogin;

				if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
				{
					providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
				}
			}
		}

		return new LoginViewModel
		{
			AllowRememberLogin = AccountOptions.AllowRememberLogin,
			EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
			ReturnUrl = returnUrl,
			UserName = context?.LoginHint,
			ExternalProviders = providers.ToArray()
		};
	}

	private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
	{
		var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
		vm.UserName = model.UserName;
		vm.RememberLogin = model.RememberLogin;
		return vm;
	}

	private async Task<RegisterViewModel> BuildRegisterViewModelAsync(RegisterInputModel model)
	{
		var vm = await BuildRegisterViewModelAsync(model.ReturnUrl);
		vm.UserName = model.UserName;
		vm.Email = model.Email;
		vm.RememberLogin = model.RememberLogin;
		return vm;
	}

	private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
	{
		var vm = new LogoutViewModel { LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt };

		if (User?.Identity.IsAuthenticated != true)
		{
			vm.ShowLogoutPrompt = false;
			return vm;
		}

		var context = await _interaction.GetLogoutContextAsync(logoutId);

		if (context?.ShowSignoutPrompt == false)
		{
			vm.ShowLogoutPrompt = false;
			return vm;
		}

		return vm;
	}

	private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
	{
		var logout = await _interaction.GetLogoutContextAsync(logoutId);

		var vm = new LoggedOutViewModel
		{
			AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
			PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
			ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
			SignOutIframeUrl = logout?.SignOutIFrameUrl,
			LogoutId = logoutId
		};

		if (User?.Identity.IsAuthenticated == true)
		{
			var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

			if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
			{
				var handler = await _handlerProvider.GetHandlerAsync(HttpContext, idp);
				if (handler is IAuthenticationSignOutHandler)
				{
					if (vm.LogoutId == null)
					{
						vm.LogoutId = await _interaction.CreateLogoutContextAsync();
					}

					vm.ExternalAuthenticationScheme = idp;
				}
			}
		}

		return vm;
	}

	private async Task<IActionResult> GetCancelRedirect(AuthorizationRequest context, string returnUrl)
	{
		if (context is null)
		{
			return Redirect("~/");
		}

		await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

		if (context.IsNativeClient())
		{
			return this.LoadingPage("Redirect", returnUrl);
		}

		return Redirect(returnUrl);
	}

	private async Task<IActionResult> HandleSuccessfulLoginAsync(string userName, string returnUrl, AuthorizationRequest context)
	{
		var user = await _userManager.FindByNameAsync(userName);
		await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.UserName, clientId: context?.Client.ClientId));

		if (context != null)
		{
			if (context.IsNativeClient())
			{
				return this.LoadingPage("Redirect", returnUrl);
			}

			return Redirect(returnUrl);
		}

		if (Url.IsLocalUrl(returnUrl))
		{
			return Redirect(returnUrl);
		}
		else if (string.IsNullOrEmpty(returnUrl))
		{
			return Redirect("~/");
		}
		else
		{
			throw new Exception("Некорректный returnUrl");
		}
	}

	private async Task HandleUnsuccessfulLoginAsync(string userName, bool isLockedOut, bool isNotAllowed, string clientId, string returnUrl)
	{
		var user = await _userManager.FindByNameAsync(userName);

		var isUserExist = user != null;

		if (isLockedOut)
		{
			var forgoutLink = Url.Action(nameof(ForgotPassword), "Account", new { returnUrl }, Request.Scheme);

			var content = string.Format("Ваша учетная запись заблокирована, чтобы восстановить пароль, пожалуйста, нажмите на эту ссылку: {0} ." +
				"После восстановления пароля попробуйте войти в систему снова через {1} минут.", forgoutLink, _authenticationSettings.AccountBlocking.LockoutMinutes);

			var message = new Message(new (string name, string address)[] { (user.UserName, user.Email) }, "Блокировка аккаунта", content, null);

			_emailSender.SendEmailAsync(message);

			await _events.RaiseAsync(new UserLoginFailureEvent(userName, AccountOptions.UserLockedOutErrorMessage, clientId: clientId));
			ModelState.AddModelError(string.Empty, AccountOptions.UserLockedOutErrorMessage);
		}
		else if (isUserExist && !await _userManager.IsEmailConfirmedAsync(user))
		{
			await _mediator.Publish(new UserCreatedDomainEvent(user, true, new[]
			{
				new SendEmailDto(nameof(ConfirmEmail), "Account", Url, clientId, Request.Scheme)
			}));

			await _events.RaiseAsync(new UserLoginFailureEvent(user.UserName, AccountOptions.DontEmailConfirmend, clientId: clientId));
			ModelState.AddModelError(string.Empty, AccountOptions.DontEmailConfirmend);
		}
		else
		{
			var errorMessage = new StringBuilder();

			var isConfirmedEmail = false;
			var error = string.Empty;

			if (isUserExist)
			{
				var attemptsLeft = _authenticationSettings.AccountBlocking.MaxLoginAttempts - user.AccessFailedCount;
				error = string.Format(AccountOptions.AttemptsLeftErrorMessage, attemptsLeft);
			}

			errorMessage
				.AppendLine(isNotAllowed ? AccountOptions.InvalidLoginAttempt : AccountOptions.InvalidCredentialsErrorMessage)
				.AppendLine(error);

			await _events.RaiseAsync(new UserLoginFailureEvent(userName, errorMessage.ToString(), clientId: clientId));
			ModelState.AddModelError(string.Empty, errorMessage.ToString());
		}
	}

	private async Task<string> GetClient64Async(string returnUrl = null)
	{
		if (string.IsNullOrEmpty(returnUrl))
			return null;

		var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

		if (context is null)
			return null;

		var regexMath = new Regex("(^http[s]://localhost:\\d{1,6})|(^http[s]://\\d{3}.\\d{3}.\\d{2}.\\d{2}:\\d{1,6})", RegexOptions.IgnoreCase)
			.Match(context.RedirectUri);

		if (!regexMath.Success)
			return null;

		var stend = regexMath.Groups[2].Value;

		var client = string.IsNullOrEmpty(stend) ? regexMath.Groups[1].Value : stend;

		var clientByte = Encoding.UTF8.GetBytes(client + 't');

		var client64 = Convert.ToBase64String(clientByte);

		return client64;
	}

	private string GetClient(string client64 = null)
	{
		if (string.IsNullOrEmpty(client64))
			return null;

		var clientDeByte64 = Convert.FromBase64String(client64);

		var clientDeByte = Encoding.UTF8.GetString(clientDeByte64);

		var client = clientDeByte.TrimEnd('t');

		return client;
	}
}