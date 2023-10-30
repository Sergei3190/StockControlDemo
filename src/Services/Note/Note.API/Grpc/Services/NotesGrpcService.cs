using Grpc.Core;

using MediatR;

using Note.API.Infrastructure.Mappers;
using Note.API.MediatR.Commands;

namespace GrpcNote;

public class NotesGrpcService : Note.NoteBase
{
	private readonly IMediator _mediator;
	private readonly ILogger<NotesGrpcService> _logger;

	public NotesGrpcService(IMediator mediator, ILogger<NotesGrpcService> logger)
	{
		_mediator = mediator;
		_logger = logger;
	}

	public override async Task<NoteArrayResponse> UpdateSort(NoteArrayRequest request, ServerCallContext context)
	{
		ArgumentNullException.ThrowIfNull(request, nameof(request));

		_logger.LogTrace("Запускаем Grpc метод {Method}", context.Method);

		if (request?.Items?.Count == 0)
		{
			context.Status = new Status(StatusCode.NotFound, "Переданы некорректные данные");
			return new NoteArrayResponse() { Flag = false };
		}

		var dtoArray = request!.Items.Cast<NoteArrayItemRequest>().Select(n => n.CreateDto()).ToArray();

		var result = await _mediator.Send(new UpdateSortCommand(dtoArray!));

		if (!result)
		{
			context.Status = new Status(StatusCode.NotFound, "Данные не обновлены");
			return new NoteArrayResponse() { Flag = false };
		}

		context.Status = new Status(StatusCode.OK, "Данные успешно обновлены");
		return new NoteArrayResponse() { Flag = true };
	}
}
