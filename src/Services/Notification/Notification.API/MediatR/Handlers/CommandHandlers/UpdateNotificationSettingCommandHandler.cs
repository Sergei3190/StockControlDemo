using MediatR;

using Notification.API.MediatR.Commands;
using Notification.API.Services.Interfaces;

namespace Notification.API.MediatR.Handlers.CommandHandlers;

public class UpdateNotificationSettingCommandHandler : IRequestHandler<UpdateNotificationSettingCommand, bool>
{
	private readonly INotificationSettingsService _service;

	public UpdateNotificationSettingCommandHandler(INotificationSettingsService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(UpdateNotificationSettingCommand request, CancellationToken cancellationToken)
	{
		return await _service.UpdateAsync(request.Dto).ConfigureAwait(false);
	}
}