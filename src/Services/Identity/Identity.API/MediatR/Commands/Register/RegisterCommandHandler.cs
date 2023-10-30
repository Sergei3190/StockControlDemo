using Identity.API.Domain.Entities;
using Identity.API.Domain.Events;

using MediatR;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.MediatR.Commands.Register;

/// <summary>
/// Обработчик команды регистрации пользователя в системе
/// </summary>
public class RegisterCommandHandler : IRequestHandler<RegisterCommand, IdentityResult>
{
	private readonly UserManager<User> _userManager;
	private readonly IPublisher _mediator;

	public RegisterCommandHandler(UserManager<User> userManager, IPublisher mediator)
	{
		_mediator = mediator;
		_userManager = userManager;
	}

	public async Task<IdentityResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
	{
		var dto = request.Dto;

		if (dto is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(RegisterCommand));
			throw new ArgumentNullException(error, nameof(dto));
		}

		var user = new User()
		{
			UserName = dto.Model.UserName,
			Email = dto.Model.Email,
		};

		var creation_result = await _userManager.CreateAsync(user, dto.Model.Password).ConfigureAwait(false);

		if (creation_result.Succeeded)
		{
			// присваиваем роль пользователя 
			await _userManager.AddToRoleAsync(user, Role.Users).ConfigureAwait(false);

			await _mediator.Publish(new UserCreatedDomainEvent(user, false, new[] { dto.EmailDto })).ConfigureAwait(false);
		}

		return creation_result;
	}
}