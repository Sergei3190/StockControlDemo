using MediatR;

namespace PersonalCabinet.API.MediatR.Commands.UserPerson;

/// <summary>
/// Команда удаления персоны пользователя
/// </summary>
public record DeleteUserPersonCommand(Guid Id) : IRequest<bool>
{
}