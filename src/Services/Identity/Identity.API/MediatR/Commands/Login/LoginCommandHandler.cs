using Identity.API.Domain.Entities;
using Identity.API.Domain.Events;
using Identity.API.MediatR.Commands.Login;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.MediatR.Commands.UserLockout;

/// <summary>
/// Обработчик команды входа пользователя в систему
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, SignInResult>
{
	private readonly SignInManager<User> _signInManager;
	private readonly UserManager<User> _userManager;
	private readonly IPublisher _mediator;

	public LoginCommandHandler(SignInManager<User> signInManager, UserManager<User> userManager, IPublisher mediator)
	{
		_signInManager = signInManager;
		_userManager = userManager;
		_mediator = mediator;
	}

	public async Task<SignInResult> Handle(LoginCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Dto;

		if (dto is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(LoginCommand));
			throw new ArgumentNullException(error, nameof(dto));
		}

		var result = await _signInManager.PasswordSignInAsync(dto.Model.UserName, dto.Model.Password, dto.Model.RememberLogin, lockoutOnFailure: true)
			.ConfigureAwait(false);

		if (result.IsLockedOut)
		{
			var user = await _userManager.FindByNameAsync(dto.Model.UserName)
				.ConfigureAwait(false);

			await _mediator.Publish(new UserLockoutChangedDomainEvent(user, true))
				.ConfigureAwait(false);
		}

		return result;
	}
}