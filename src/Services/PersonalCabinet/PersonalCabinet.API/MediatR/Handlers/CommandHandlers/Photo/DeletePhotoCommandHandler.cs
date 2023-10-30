using MediatR;

using PersonalCabinet.API.MediatR.Commands.Photo;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.Photo;

public class DeletePhotoCommandHandler : IRequestHandler<DeletePhotoCommand, bool>
{
	private readonly IPhotosService _service;

	public DeletePhotoCommandHandler(IPhotosService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
	{
		return await _service.DeleteAsync(request.Id).ConfigureAwait(false);
	}
}