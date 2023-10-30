using MediatR;

using Notification.API.MediatR.Commands;
using Notification.API.Services.Interfaces;

namespace Notification.API.MediatR.Handlers.CommandHandlers;

public class DeleteNotificationSettingCommandHandler : IRequestHandler<DeleteNotificationSettingCommand, bool>
{
	private readonly INotificationSettingsService _service;

	public DeleteNotificationSettingCommandHandler(INotificationSettingsService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(DeleteNotificationSettingCommand request, CancellationToken cancellationToken)
	{
		return await _service.DeleteAsync(request.Id).ConfigureAwait(false);
	}
}