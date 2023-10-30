using Notification.API.Domain.Notice;
using Notification.API.Models.DTO;
using Notification.API.Models.DTO.NotificationSetting;
using Notification.API.Models.DTO.NotificationType;

using Service.Common.DTO.Entities.Base;

namespace Notification.API.Infrastructure.Mappers;

public static class NotificationSettingMapper
{
	public static NotificationSetting? CreateEntity(this NotificationSettingDto dto, Guid userId) => dto is null
		? null
		: new NotificationSetting()
		{
			Id = dto.Id,
			NotificationTypeId = dto.NotificationType.Id,
			Enable = dto.Enable,
			UserId = userId,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this NotificationSetting entity, NotificationSettingDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.Enable = dto.Enable;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static NotificationSettingDto? CreateDto(this NotificationSetting entity) => entity is null
		? null
		: new NotificationSettingDto()
		{
			Id = entity.Id,
			NotificationType = new NamedEntityDto()
			{
				Id = entity.NotificationType.Id,
				Name = entity.NotificationType.Name
			},
			Enable = entity.Enable,
		};
}
