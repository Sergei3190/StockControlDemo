using MediatR;

using PersonalCabinet.API.MediatR.Queries.Photo;
using PersonalCabinet.API.Models.DTO.Photo;
using PersonalCabinet.API.Services.Interfaces;

using Service.Common.DTO;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.Photo;

public class GetPhotosHandler : IRequestHandler<GetPhotosQuery, PaginatedItemsDto<PhotoDto>>
{
	private readonly IPhotosService _service;

	public GetPhotosHandler(IPhotosService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<PhotoDto>> Handle(GetPhotosQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}