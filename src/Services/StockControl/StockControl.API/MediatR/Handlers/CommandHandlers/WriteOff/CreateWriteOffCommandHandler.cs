using MediatR;

using StockControl.API.Domain.Events.WriteOff;
using StockControl.API.MediatR.Commands.WriteOff;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.MediatR.CommandHandlers.WriteOff;

public class CreateWriteOffCommandHandler : IRequestHandler<CreateWriteOffCommand, Guid>
{
	private readonly IWriteOffsService _service;
	private readonly IReceiptsService _receiptService;
	private readonly IPublisher _mediator;

	public CreateWriteOffCommandHandler(IWriteOffsService service, IReceiptsService receiptService, IPublisher mediator)
	{
		_service = service;
		_receiptService = receiptService;
		_mediator = mediator;
	}

	public async Task<Guid> Handle(CreateWriteOffCommand request, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(request, nameof(request));

		var dto = request.Dto;

		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		await CheckDateTimeCreatedAsync(dto.Party?.Id, dto.CreateDate, dto.CreateTime).ConfigureAwait(false);

		var result = await _service.CreateAsync(dto).ConfigureAwait(false);

		if (result != Guid.Empty)
			await _mediator.Publish(new WriteOffCreatedDomainEvent(result)).ConfigureAwait(false);

		return result;
	}

	private async Task CheckDateTimeCreatedAsync(Guid? partyId, DateOnly createDate, TimeOnly createTime)
	{
		var info = await _receiptService.GetDataTimeCreatedByPartyIdAsync(partyId!.Value).ConfigureAwait(false);

		if (!info.HasValue)
			throw new InvalidDataException($"Невозможно найти дату и время создания партии id: {partyId}");

		var receiptCreateTime = new TimeOnly(info.Value.CreateTime.Hour, info.Value.CreateTime.Minute);

		var checkDate = createDate == info.Value.CreateDate;
		var checkTime = createTime.Ticks == receiptCreateTime.Ticks;

		if ((checkDate && checkTime) || (checkDate && createTime.Ticks < receiptCreateTime.Ticks)
			|| (createDate < info.Value.CreateDate))
			throw new InvalidDataException($"Дата и время создания списания не могут быть" +
				$" равны/раньше даты и времени создания поступления партии id: {partyId}." +
			    $"Даты и время создания поступления партии: {info.Value.CreateDate} {receiptCreateTime}");
	}
}