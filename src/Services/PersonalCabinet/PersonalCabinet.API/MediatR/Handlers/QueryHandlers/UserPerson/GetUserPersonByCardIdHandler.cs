using MediatR;

using PersonalCabinet.API.MediatR.Queries.UserPerson;
using PersonalCabinet.API.Models.DTO.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.UserPerson;

public class GetUserPersonByCardIdHandler : IRequestHandler<GetUserPersonByCardIdQuery, UserPersonDto?>
{
	private readonly IUserPersonsService _service;

	public GetUserPersonByCardIdHandler(IUserPersonsService service)
	{
		_service = service;
	}

	public async Task<UserPersonDto?> Handle(GetUserPersonByCardIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByCardIdAsync(request.Id).ConfigureAwait(false);
	}
}