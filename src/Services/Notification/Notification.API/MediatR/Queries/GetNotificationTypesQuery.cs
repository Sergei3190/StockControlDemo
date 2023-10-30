using MediatR;

using Notification.API.Models.DTO.NotificationType;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace Notification.API.MediatR.Queries;

/// <summary>
/// Запрос на получение отфильтрованных типов уведомления
/// </summary>
public record GetNotificationTypesQuery(NotificationTypeFilterDto Filter) : IRequest<PaginatedItemsDto<NamedEntityDto>>
{
}
