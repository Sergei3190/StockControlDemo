using FileStorage.API.MediatR.Commands;
using FileStorage.API.Models.Events;
using FileStorage.API.Services.Interfaces;

using MediatR;

namespace FileStorage.API.MediatR.Handlers.CommandHandlers;

public class UnloadFileCommandHandler : IRequestHandler<UnloadFileCommand, Guid>
{
	private readonly IPublisher _mediator;
	private readonly IFileStorageService _filesService;

	public UnloadFileCommandHandler(IPublisher mediator, IFileStorageService filesService)
	{
		_mediator = mediator;
		_filesService = filesService;
	}

	public async Task<Guid> Handle(UnloadFileCommand request, CancellationToken cancellationToken)
	{
		var file = request.FileInfo;
		var content = request.Content;

		var id = await _filesService.UnloadAsync(file, content).ConfigureAwait(false);

		if (id == file.Id)
			await _mediator.Publish(new StorageFileInfoUploadDomainEvent(file.Id)).ConfigureAwait(false);

		return id;
	}
}