using Email.Service;

using Identity.API.Domain.Entities;
using Identity.API.Domain.Events;
using Identity.API.Models.DTO;

using MediatR;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Services;

/// <summary>
/// Обработчик доменного события создания пользователя, который выполняет отправку токена подтверждения почты пользователя
/// </summary>
public class SendEmailConfirmationToken
	: INotificationHandler<UserCreatedDomainEvent>
{
	// логгер не регистрируем, тк у нас уже есть регистрация запроса и его ответа в LoggingBehavior, в том числе при возникновении ошибки.
	// логгер добавляем только если нужно что то дополнительно зарегистрировать внутри обработчика
	private readonly UserManager<User> _userManager;
	private readonly IEmailSender _emailSender;

	public SendEmailConfirmationToken(
		UserManager<User> userManager,
		IEmailSender emailSender)
	{
		_userManager = userManager;
		_emailSender = emailSender;
	}

	public async Task Handle(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));

		var user = domainEvent.User;
		var parameters = domainEvent.Parameters.Where(p => p.GetType().Equals(typeof(SendEmailDto))).First() as SendEmailDto;

		if (user is null || parameters is null)
		{
			var error = "Отсутствуют данные для отправки электронной почты";
			throw new ArgumentNullException(error, nameof(user));
		}

		var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
		var confirmationLink = parameters.Url.Action(parameters.Action, parameters.Controller, new { token, email = user.Email, parameters.ClientId }, parameters.Scheme);

		var message = new Message(new (string name, string address)[] { (user.UserName, user.Email) }, "Подтверждение электронной почты", confirmationLink, null);

		// В случаи если письмо не дойдёт до пользователя или пользователь его удалит, мы всегда сможем вновь отправить токен подтверждения повторно
		// https://learn.microsoft.com/en-us/aspnet/mvc/overview/security/create-an-aspnet-mvc-5-web-app-with-email-confirmation-and-password-reset#resend-email-confirmation-link
		_emailSender.SendEmailAsync(message);
	}
}
