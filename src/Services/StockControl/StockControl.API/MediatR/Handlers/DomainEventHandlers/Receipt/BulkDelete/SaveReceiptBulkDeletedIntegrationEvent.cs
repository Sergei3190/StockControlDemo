using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.DTO;
using Service.Common.Integration.Events.StockControl.Receipt;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Receipt.BulkDelete;

/// <summary>
/// Обработчик доменного события массового удаления поступления
/// </summary>
public class SaveReceiptBulkDeletedIntegrationEvent
	: INotificationHandler<ReceiptBulkDeletedDomainEvent>
{
	private readonly IReceiptsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveReceiptBulkDeletedIntegrationEvent(
		IReceiptsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(ReceiptBulkDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.ReceiptIds).ConfigureAwait(false);

		if (!data.Any())
			return;

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new ReceiptBulkDeletedIntegrationEvent(data.Cast<(Guid Id, string Number)>()
			.Select(d => new ProductFlowInfoDto() { ProductFlowId = d.Id, Number = d.Number } ), userName);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
