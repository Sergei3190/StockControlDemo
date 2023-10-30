using Service.Common.DTO.Entities.Base;

namespace Notification.API.Models.DTO.NotificationSetting;

/// <summary>
/// Настройки уведомления
/// </summary>
public class NotificationSettingDto : EntityDto
{
	/// <summary>
	/// Тип уведомления
	/// </summary>
	public NamedEntityDto NotificationType { get; set; }

	/// <summary>
	/// Признак включенного уведомления
	/// </summary>
	public bool Enable { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(NotificationType)}: {NotificationType.Id} {NotificationType.Name} " +
		$" {nameof(Enable)}: {Enable}";
}