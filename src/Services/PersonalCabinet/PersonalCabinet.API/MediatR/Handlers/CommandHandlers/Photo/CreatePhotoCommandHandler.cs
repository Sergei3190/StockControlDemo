using MediatR;

using PersonalCabinet.API.MediatR.Commands.Photo;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.Photo;

public class CreatePhotoCommandHandler : IRequestHandler<CreatePhotoCommand, Guid>
{
	private readonly IPhotosService _service;

	public CreatePhotoCommandHandler(IPhotosService service)
	{
		_service = service;
	}

	public async Task<Guid> Handle(CreatePhotoCommand request, CancellationToken cancellationToken)
	{
		return await _service.CreateAsync(request.Dto).ConfigureAwait(false);
	}
}