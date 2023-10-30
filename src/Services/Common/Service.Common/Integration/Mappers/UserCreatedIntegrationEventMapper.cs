using Service.Common.Entities.App;
using Service.Common.Integration.Events.Identity;

namespace Service.Common.Integration.Mappers;

public static class UserCreatedIntegrationEventMapper
{
    public static UserInfo? ToUserInfo(this UserCreatedIntegrationEvent? @event) => @event is null
        ? null
        : new UserInfo() { Id = @event.UserId, Name = @event.Name, Email = @event.Email, SourceId = @event.SourceId };
}
