using MediatR;

using PersonalCabinet.API.MediatR.Commands.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.UserPerson;

public class UpdateUserPersonCommandHandler : IRequestHandler<UpdateUserPersonCommand, bool>
{
	private readonly IUserPersonsService _service;

	public UpdateUserPersonCommandHandler(IUserPersonsService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(UpdateUserPersonCommand request, CancellationToken cancellationToken)
	{
		return await _service.UpdateAsync(request.Dto).ConfigureAwait(false);
	}
}