using MediatR;

using PersonalCabinet.API.MediatR.Commands.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

namespace PersonalCabinet.API.MediatR.CommandHandlers.UserPerson;

public class DeleteUserPersonCommandHandler : IRequestHandler<DeleteUserPersonCommand, bool>
{
	private readonly IUserPersonsService _service;

	public DeleteUserPersonCommandHandler(IUserPersonsService service)
	{
		_service = service;
	}

	public async Task<bool> Handle(DeleteUserPersonCommand request, CancellationToken cancellationToken)
	{
		return await _service.DeleteAsync(request.Id).ConfigureAwait(false);
	}
}