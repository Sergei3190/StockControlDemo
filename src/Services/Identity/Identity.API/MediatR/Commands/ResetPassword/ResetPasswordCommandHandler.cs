using Identity.API.Domain.Entities;
using Identity.API.Domain.Events;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.MediatR.Commands.ResetPassword;

/// <summary>
/// Обработчик команды сброса пароля пользователя
/// </summary>
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IdentityResult>
{
	private readonly UserManager<User> _userManager;
	private readonly IPublisher _mediator;

	public ResetPasswordCommandHandler(UserManager<User> userManager, IPublisher mediator)
	{
		_userManager = userManager;
		_mediator = mediator;
	}

	public async Task<IdentityResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Dto;

		if (dto is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(ResetPasswordCommand));
			throw new ArgumentNullException(error, nameof(dto));
		}

		var user = await _userManager.FindByNameAsync(dto.UserName)
			.ConfigureAwait(false);

		var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password)
			.ConfigureAwait(false);

		if (result.Succeeded)
		{
			await _mediator.Publish(new UserLockoutChangedDomainEvent(user, false))
				.ConfigureAwait(false);
		}

		return result;
	}
}