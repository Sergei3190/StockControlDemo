using MediatR;

using PersonalCabinet.API.MediatR.Queries.UserPerson;
using PersonalCabinet.API.Models.DTO.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.UserPerson;

public class GetUserPersonCurrentUserHandler : IRequestHandler<GetUserPersonCurrentUserQuery, UserPersonDto?>
{
	private readonly IUserPersonsService _service;

	public GetUserPersonCurrentUserHandler(IUserPersonsService service)
	{
		_service = service;
	}

	public async Task<UserPersonDto?> Handle(GetUserPersonCurrentUserQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetPersonCurrentUserAsync().ConfigureAwait(false);
	}
}