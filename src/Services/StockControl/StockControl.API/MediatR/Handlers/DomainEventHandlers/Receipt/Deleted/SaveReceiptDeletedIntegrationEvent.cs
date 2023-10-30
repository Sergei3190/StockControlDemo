using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.Events.StockControl.Receipt;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Receipt.Deleted;

/// <summary>
/// Обработчик доменного события удаления поступления
/// </summary>
public class SaveReceiptDeletedIntegrationEvent
	: INotificationHandler<ReceiptDeletedDomainEvent>
{
	private readonly IReceiptsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveReceiptDeletedIntegrationEvent(
		IReceiptsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(ReceiptDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.ReceiptId).ConfigureAwait(false);

		var singlData = data.First();

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new ReceiptDeletedIntegrationEvent(singlData.Id, singlData.Number, userName);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
