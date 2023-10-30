using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.Events.StockControl.Moving;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.Moving;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Moving.Deleted;

/// <summary>
/// Обработчик доменного события удаления перемещения
/// </summary>
public class SaveMovingDeletedIntegrationEvent
	: INotificationHandler<MovingDeletedDomainEvent>
{
	private readonly IMovingsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveMovingDeletedIntegrationEvent(
		IMovingsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(MovingDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.MovingId).ConfigureAwait(false);

		var singlData = data.First();

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new MovingDeletedIntegrationEvent(singlData.Id, singlData.Number, userName);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
