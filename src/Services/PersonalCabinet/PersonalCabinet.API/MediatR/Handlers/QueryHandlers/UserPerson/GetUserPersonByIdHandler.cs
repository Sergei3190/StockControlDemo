using MediatR;

using PersonalCabinet.API.MediatR.Queries.UserPerson;
using PersonalCabinet.API.Models.DTO.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.UserPerson;

public class GetUserPersonByIdHandler : IRequestHandler<GetUserPersonByIdQuery, UserPersonDto?>
{
	private readonly IUserPersonsService _service;

	public GetUserPersonByIdHandler(IUserPersonsService service)
	{
		_service = service;
	}

	public async Task<UserPersonDto?> Handle(GetUserPersonByIdQuery request, CancellationToken cancellationToken)
	{
		return await _service.GetByIdAsync(request.Id).ConfigureAwait(false);
	}
}