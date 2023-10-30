using MediatR;
using Notification.API.Models.DTO.NotificationSetting;

namespace Notification.API.MediatR.Queries;

/// <summary>
/// Запрос на получение настройки уведомления по id
/// </summary>
/// <param name="Id"></param>
public record GetNotificationSettingByIdQuery(Guid Id) : IRequest<NotificationSettingDto?>
{
}
