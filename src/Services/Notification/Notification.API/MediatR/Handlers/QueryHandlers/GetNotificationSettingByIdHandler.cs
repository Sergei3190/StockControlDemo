using MediatR;

using Notification.API.MediatR.Queries;
using Notification.API.Models.DTO.NotificationSetting;
using Notification.API.Services.Interfaces;

namespace Notification.API.MediatR.Handlers.QueryHandlers;

public class GetNotificationSettingByIdHandler : IRequestHandler<GetNotificationSettingByIdQuery, NotificationSettingDto?>
{
	private readonly INotificationSettingsService _service;

	public GetNotificationSettingByIdHandler(INotificationSettingsService service)
	{
		_service = service;
	}

	public async Task<NotificationSettingDto?> Handle(GetNotificationSettingByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}