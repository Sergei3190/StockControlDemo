using MediatR;

using Notification.API.MediatR.Queries;
using Notification.API.Models.DTO.NotificationSetting;
using Notification.API.Services.Interfaces;

using Service.Common.DTO;

namespace Notification.API.MediatR.Handlers.QueryHandlers;

public class GetNotificationSettingsHandler : IRequestHandler<GetNotificationSettingsQuery, PaginatedItemsDto<NotificationSettingDto>>
{
	private readonly INotificationSettingsService _service;

	public GetNotificationSettingsHandler(INotificationSettingsService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<NotificationSettingDto>> Handle(GetNotificationSettingsQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}