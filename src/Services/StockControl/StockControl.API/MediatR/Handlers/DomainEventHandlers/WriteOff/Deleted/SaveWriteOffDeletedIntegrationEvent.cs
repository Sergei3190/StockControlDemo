﻿using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.Events.StockControl.WriteOff;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.WriteOff.Deleted;

/// <summary>
/// Обработчик доменного события удаления списания
/// </summary>
public class SaveWriteOffDeletedIntegrationEvent
	: INotificationHandler<WriteOffDeletedDomainEvent>
{
	private readonly IWriteOffsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveWriteOffDeletedIntegrationEvent(
		IWriteOffsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(WriteOffDeletedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.WriteOffId).ConfigureAwait(false);

		var singlData = data.First();

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new WriteOffDeletedIntegrationEvent(singlData.Id, singlData.Number, userName);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
