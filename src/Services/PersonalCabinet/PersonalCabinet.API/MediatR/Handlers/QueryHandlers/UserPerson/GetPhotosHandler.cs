using MediatR;

using PersonalCabinet.API.MediatR.Queries.UserPerson;
using PersonalCabinet.API.Models.DTO.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

using Service.Common.DTO;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.UserPerson;

public class GetUserPersonsHandler : IRequestHandler<GetUserPersonsQuery, PaginatedItemsDto<UserPersonDto>>
{
	private readonly IUserPersonsService _service;

	public GetUserPersonsHandler(IUserPersonsService service)
	{
		_service = service;
	}

	public async Task<PaginatedItemsDto<UserPersonDto>> Handle(GetUserPersonsQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetListAsync(request.Filter).ConfigureAwait(false);
	}
}