using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.Events.StockControl.Receipt;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Receipt.Updated;

/// <summary>
/// Обработчик доменного события обновления поступления
/// </summary>
public class SaveReceiptUpdatedIntegrationEvent
	: INotificationHandler<ReceiptUpdatedDomainEvent>
{
	private readonly IReceiptsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveReceiptUpdatedIntegrationEvent(
		IReceiptsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(ReceiptUpdatedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.ReceiptId).ConfigureAwait(false);

		var singlData = data.First();

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new ReceiptUpdatedIntegrationEvent(singlData.Id, singlData.Number, userName);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
