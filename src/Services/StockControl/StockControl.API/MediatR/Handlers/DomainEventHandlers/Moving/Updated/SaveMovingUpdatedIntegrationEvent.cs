using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.Events.StockControl.Moving;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.Moving;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Moving.Updated;

/// <summary>
/// Обработчик доменного события обновления перемещения
/// </summary>
public class SaveMovingUpdatedIntegrationEvent
	: INotificationHandler<MovingUpdatedDomainEvent>
{
	private readonly IMovingsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveMovingUpdatedIntegrationEvent(
		IMovingsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(MovingUpdatedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.MovingId).ConfigureAwait(false);

		var singlData = data.First();

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new MovingUpdatedIntegrationEvent(singlData.Id, singlData.Number, userName);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
