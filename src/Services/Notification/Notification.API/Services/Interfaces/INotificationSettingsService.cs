using Notification.API.Models.DTO.NotificationSetting;
using Service.Common.Interfaces;

namespace Notification.API.Services.Interfaces;

/// <summary>
/// Сервис настройки уведомлений
/// </summary>
public interface INotificationSettingsService : ICrudService<NotificationSettingDto, NotificationSettingFilterDto>
{
}