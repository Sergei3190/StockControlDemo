using Notification.API.Domain.Notice;

using Service.Common.DTO.Entities.Base;

namespace Notification.API.Infrastructure.Mappers;

public static class NotificationTypeMapper
{
	public static NamedEntityDto? CreateDto(this NotificationType entity) => entity is null
		? null
		: new NamedEntityDto()
		{
			Id = entity.Id,
			Name = entity.Name
		};
}
