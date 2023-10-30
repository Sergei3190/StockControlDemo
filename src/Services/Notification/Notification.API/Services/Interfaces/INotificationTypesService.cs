using Notification.API.Models.DTO.NotificationType;

using Service.Common.DTO.Entities.Base;
using Service.Common.Interfaces;

namespace Notification.API.Services.Interfaces;

/// <summary>
/// Сервис типов уведомлений
/// </summary>
public interface INotificationTypesService : ISelectService<NamedEntityDto, NotificationTypeFilterDto>
{
}