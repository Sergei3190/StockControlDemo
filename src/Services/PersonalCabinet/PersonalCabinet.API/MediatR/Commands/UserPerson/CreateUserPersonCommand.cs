using MediatR;

using PersonalCabinet.API.Models.DTO.UserPerson;

namespace PersonalCabinet.API.MediatR.Commands.UserPerson;

/// <summary>
/// Команда создания персоны пользователя
/// </summary>
public record CreateUserPersonCommand(UserPersonDto? Dto) : IRequest<Guid>
{
}