using Identity.API.DAL.Context;
using Identity.API.Domain.Events;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Service.Common.Integration;
using Service.Common.Integration.Events.Identity;

namespace Identity.API.Services;

public class UserLockoutChangedDomainEventHandler
	: INotificationHandler<UserLockoutChangedDomainEvent>
{
	// логгер не регистрируем, тк у нас уже есть регистрация запроса и его ответа в LoggingBehavior, в том числе при возникновении ошибки.
	// логгер добавляем только если нужно что то дополнительно зарегистрировать внутри обработчика
	private readonly IdentityDB _db;
	private readonly IIntegrationEventService _integrationService;

	public UserLockoutChangedDomainEventHandler(
		IdentityDB db,
		IIntegrationEventService integrationService)
	{
		_db = db ?? throw new ArgumentNullException(nameof(db));
		_integrationService = integrationService;
	}

	public async Task Handle(UserLockoutChangedDomainEvent domainEvent, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));

		var user = await _db.Users
			.AsNoTracking()
			.Where(u => u.Id == domainEvent.User.Id)
			.SingleOrDefaultAsync()
			.ConfigureAwait(false);

		if (user is null)
		{
			var error = string.Format("Пользователь {0} не найден в системе", domainEvent.User.UserName);
			throw new ArgumentNullException(error, nameof(domainEvent.User));
		}

		var integrationEvent = new UserLockoutChangedIntegrationEvent(user.Id, domainEvent.IsLockout);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
