using Service.Common.DTO;

namespace Notification.API.Models.DTO.NotificationSetting;

/// <summary>
/// Фильтр настройки уведомления
/// </summary>
public class NotificationSettingFilterDto : FilterDto
{
	public Guid? NotificationTypeId { get; set; }
}