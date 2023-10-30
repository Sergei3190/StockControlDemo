using MediatR;

using PersonalCabinet.API.MediatR.Queries.Photo;
using PersonalCabinet.API.Models.DTO.Photo;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.Photo;

public class GetPhotoByCardIdHandler : IRequestHandler<GetPhotoByCardIdQuery, PhotoDto?>
{
	private readonly IPhotosService _service;

	public GetPhotoByCardIdHandler(IPhotosService service)
	{
		_service = service;
	}

	public async Task<PhotoDto?> Handle(GetPhotoByCardIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByCardIdAsync(request.Id).ConfigureAwait(false);
	}
}