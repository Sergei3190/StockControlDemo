using MediatR;

using StockControl.API.Domain.Events.Receipt;
using StockControl.API.MediatR.Commands.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.Receipt;

public class CreateReceiptCommandHandler : IRequestHandler<CreateReceiptCommand, Guid>
{
	private readonly IReceiptsService _service;
	private readonly IPartiesService _partiesService;
	private readonly IPublisher _mediator;

	public CreateReceiptCommandHandler(IReceiptsService service, IPartiesService partiesService, IPublisher mediator)
	{
		_service = service;
		_partiesService = partiesService;
		_mediator = mediator;
	}

	public async Task<Guid> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request, nameof(request));

		var dto = request.Dto;

		if (dto is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(CreateReceiptCommand));
			throw new ArgumentNullException(error, nameof(dto));
		}

		// не выносим в отдельную команду, тк 
		// сохранение партии и документа поступления должно быть атомарной операцией
		dto.Party.Id = await _partiesService.CreateAsync(dto.Party).ConfigureAwait(false);

		var result = await _service.CreateAsync(dto).ConfigureAwait(false);

		if (result != Guid.Empty)
			await _mediator.Publish(new ReceiptCreatedDomainEvent(result)).ConfigureAwait(false);

		return result;
	}
}