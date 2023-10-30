﻿using MediatR;

using Service.Common.Integration;
using Service.Common.Integration.Events.StockControl.WriteOff;
using Service.Common.Interfaces;

using StockControl.API.Domain.Events.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.Handlers.DomainEventHandlers.WriteOff.Created;

/// <summary>
/// Обработчик доменного события создания списания
/// </summary>
public class SaveWriteOffCreatedIntegrationEvent
	: INotificationHandler<WriteOffCreatedDomainEvent>
{
	private readonly IWriteOffsService _service;
	private readonly IIntegrationEventService _integrationService;
	private readonly IIdentityService _identityService;

	public SaveWriteOffCreatedIntegrationEvent(
		IWriteOffsService service,
		IIntegrationEventService integrationService,
		IIdentityService identityService)
	{
		_service = service;
		_integrationService = integrationService;
		_identityService = identityService;
	}

	public async Task Handle(WriteOffCreatedDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		var data = await _service.GetIntegrationData(@event.WriteOffId).ConfigureAwait(false);

		var singlData = data.First();

		var userName = _identityService.GetUserNameIdentity();

		var integrationEvent = new WriteOffCreatedIntegrationEvent(singlData.Id, singlData.Number, userName);
		await _integrationService.AddAndSaveEventAsync(integrationEvent).ConfigureAwait(false);
	}
}
