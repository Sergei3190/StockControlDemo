using MediatR;
using Notification.API.Models.DTO.NotificationSetting;

namespace Notification.API.MediatR.Commands;

/// <summary>
/// Команда обновления настройки уведомления
/// </summary>
public record UpdateNotificationSettingCommand(NotificationSettingDto? Dto) : IRequest<bool>
{
}