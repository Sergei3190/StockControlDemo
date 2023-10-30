using Identity.API.Domain.Entities;

using MediatR;

namespace Identity.API.Domain.Events;

/// <summary>
/// Доменное событие создания пользователя
/// </summary>
/// <param name="User">Доменный объект пользователя</param>
/// <param name="Parameters">дополнительные параметры, которые могут понадобиться при отправке уведомлений, 
/// например для формирования ссылки токена подтверждения почты пользователя</param>
public record UserCreatedDomainEvent(User User, bool IsRetrySenEmail, object[]? Parameters = null) : INotification
{
}