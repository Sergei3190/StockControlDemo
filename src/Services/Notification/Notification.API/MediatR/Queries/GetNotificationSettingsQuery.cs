using MediatR;
using Notification.API.Models.DTO.NotificationSetting;
using Service.Common.DTO;

namespace Notification.API.MediatR.Queries;

/// <summary>
/// Запрос на получение отфильтрованного списка настроек уведомлений
/// </summary>
public record GetNotificationSettingsQuery(NotificationSettingFilterDto Filter) : IRequest<PaginatedItemsDto<NotificationSettingDto>>
{
}
