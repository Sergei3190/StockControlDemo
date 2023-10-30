using MediatR;

using PersonalCabinet.API.MediatR.Queries.Photo;
using PersonalCabinet.API.Models.DTO.Photo;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.Photo;

public class GetPhotoByIdHandler : IRequestHandler<GetPhotoByIdQuery, PhotoDto?>
{
	private readonly IPhotosService _service;

	public GetPhotoByIdHandler(IPhotosService service)
	{
		_service = service;
	}

	public async Task<PhotoDto?> Handle(GetPhotoByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}