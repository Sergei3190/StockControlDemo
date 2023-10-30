using Service.Common.Integration.Events.Base;

namespace Service.Common.Integration.Events.Identity;

/// <summary>
/// Интеграционное событие изменения признака блокировки пользователя, обработчик будет в каждом сервисе свой
/// </summary>
public class UserLockoutChangedIntegrationEvent : SCRMQIntegrationEvent
{
    public Guid UserId { get; init; }
    public bool IsLockout { get; init; }

    public UserLockoutChangedIntegrationEvent(Guid userId, bool isLockout)
    {
        UserId = userId;
        IsLockout = isLockout;
    }
}
