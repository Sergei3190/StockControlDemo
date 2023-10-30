using Identity.API.Domain.Entities;

using MediatR;

namespace Identity.API.Domain.Events;

/// <summary>
/// Доменное событие изменения статуса блокировки пользователя
/// </summary>
public record UserLockoutChangedDomainEvent(User User, bool IsLockout) : INotification
{
}