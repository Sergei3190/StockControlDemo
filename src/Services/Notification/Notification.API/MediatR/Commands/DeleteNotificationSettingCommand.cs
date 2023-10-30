using MediatR;

namespace Notification.API.MediatR.Commands;

/// <summary>
/// Команда удаления настройки уведомления
/// </summary>
public record DeleteNotificationSettingCommand(Guid Id) : IRequest<bool>
{
}