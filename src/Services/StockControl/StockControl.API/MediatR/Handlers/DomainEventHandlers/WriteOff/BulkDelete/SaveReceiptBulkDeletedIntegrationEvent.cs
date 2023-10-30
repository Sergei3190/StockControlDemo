using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.DTO;
using Service.Common.Integration.Events.StockControl.WriteOff;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.WriteOff.BulkDelete;

/// <summary>
/// Обработчик доменного события массового удаления списания
/// </summary>
public class SaveWriteOffBulkDeletedIntegrationEvent
	: INotificationHandler<WriteOffBulkDeletedDomainEvent>
{
	private readonly IWriteOffsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveWriteOffBulkDeletedIntegrationEvent(
		IWriteOffsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(WriteOffBulkDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.WriteOffIds).ConfigureAwait(false);

		if (!data.Any())
			return;

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new WriteOffBulkDeletedIntegrationEvent(data.Cast<(Guid Id, string Number)>()
			.Select(d => new ProductFlowInfoDto() { ProductFlowId = d.Id, Number = d.Number }), userName);

		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
