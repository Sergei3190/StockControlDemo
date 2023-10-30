using MediatR;

using Notification.API.MediatR.Commands;
using Notification.API.Services.Interfaces;

namespace Notification.API.MediatR.Handlers.CommandHandlers;

public class CreateNotificationSettingCommandHandler : IRequestHandler<CreateNotificationSettingCommand, Guid>
{
	private readonly INotificationSettingsService _service;

	public CreateNotificationSettingCommandHandler(INotificationSettingsService service)
	{
		_service = service;
	}

	public async Task<Guid> Handle(CreateNotificationSettingCommand request, CancellationToken cancellationToken)
	{
		return await _service.CreateAsync(request.Dto).ConfigureAwait(false);
	}
}