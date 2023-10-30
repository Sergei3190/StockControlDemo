using System.Linq;

using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.DTO;
using Service.Common.Integration.Events.StockControl.Moving;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.Moving;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.Moving.BulkDelete;

/// <summary>
/// Обработчик доменного события массового удаления перемещения
/// </summary>
public class SaveMovingBulkDeletedIntegrationEvent
	: INotificationHandler<MovingBulkDeletedDomainEvent>
{
	private readonly IMovingsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveMovingBulkDeletedIntegrationEvent(
		IMovingsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(MovingBulkDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.MovingIds).ConfigureAwait(false);

		if (!data.Any())
			return;

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new MovingBulkDeletedIntegrationEvent(data.Cast<(Guid Id, string Number)>()
			.Select(d => new ProductFlowInfoDto() { ProductFlowId = d.Id, Number = d.Number }), userName);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
