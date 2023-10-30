using MediatR;
using Notification.API.Models.DTO.NotificationSetting;

namespace Notification.API.MediatR.Commands;

/// <summary>
/// Команда создания настройки уведомления
/// </summary>
public record CreateNotificationSettingCommand(NotificationSettingDto? Dto) : IRequest<Guid>
{
}