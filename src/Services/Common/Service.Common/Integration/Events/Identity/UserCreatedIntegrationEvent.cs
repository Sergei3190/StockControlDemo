using Service.Common.Integration.Events.Base;

namespace Service.Common.Integration.Events.Identity;

/// <summary>
/// Интеграционное событие создания пользователя, обработчик будет в каждом сервисе свой
/// </summary>
public class UserCreatedIntegrationEvent : SCRMQIntegrationEvent
{
    public Guid UserId { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }

    public UserCreatedIntegrationEvent(Guid userId, string name, string email)
    {
        UserId = userId;
        Name = name;
        Email = email;
    }
}
