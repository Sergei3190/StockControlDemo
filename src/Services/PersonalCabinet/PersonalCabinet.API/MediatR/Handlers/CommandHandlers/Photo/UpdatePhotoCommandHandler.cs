using MediatR;

using PersonalCabinet.API.MediatR.Commands.Photo;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.Photo;

public class UpdatePhotoCommandHandler : IRequestHandler<UpdatePhotoCommand, bool>
{
	private readonly IPhotosService _service;

	public UpdatePhotoCommandHandler(IPhotosService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(UpdatePhotoCommand request, CancellationToken cancellationToken)
	{
		return await _service.UpdateAsync(request.Dto).ConfigureAwait(false);
	}
}