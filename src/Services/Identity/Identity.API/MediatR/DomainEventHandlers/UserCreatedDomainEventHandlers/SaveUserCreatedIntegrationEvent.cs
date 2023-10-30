using Identity.API.DAL.Context;
using Identity.API.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Service.Common.Integration;
using Service.Common.Integration.Events.Identity;

namespace Identity.API.MediatR.DomainEventHandlers.UserDomainEventHandlers;

/// <summary>
/// Обработчик доменного события создания пользователя, который выполняет сохранение в БД интеграционного события создания пользователя 
/// </summary>
public class SaveUserCreatedIntegrationEvent
	: INotificationHandler<UserCreatedDomainEvent>
{
	// логгер не регистрируем, тк у нас уже есть регистрация запроса и его ответа в LoggingBehavior, в том числе при возникновении ошибки.
	// логгер добавляем только если нужно что то дополнительно зарегистрировать внутри обработчика
	private readonly IdentityDB _db;
	private readonly IIntegrationEventService _integrationService;

	public SaveUserCreatedIntegrationEvent(
		IdentityDB db,
		IIntegrationEventService integrationService)
	{
		_db = db ?? throw new ArgumentNullException(nameof(db));
		_integrationService = integrationService;
	}

	public async Task Handle(UserCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));

		if (domainEvent.IsRetrySenEmail)
			return;

		// не проверяем на подтверждение почты, тк пользователь может подтвердить её неизвестно когда
		var user = await _db.Users
			.AsNoTracking()
			.Where(u => u.Id == domainEvent.User.Id)
			.Where(u => !u.LockoutEnd.HasValue)
			.SingleOrDefaultAsync()
			.ConfigureAwait(false);

		if (user is null)
		{
			var error = string.Format("Пользователь {0} не найден в системе", domainEvent.User.UserName);
			throw new ArgumentNullException(error, nameof(domainEvent.User));
		}

		var integrationEvent = new UserCreatedIntegrationEvent(user.Id, user.UserName, user.Email);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
