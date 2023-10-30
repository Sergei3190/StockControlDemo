using MediatR;

using PersonalCabinet.API.Models.DTO.UserPerson;

namespace PersonalCabinet.API.MediatR.Commands.UserPerson;

/// <summary>
/// Команда обновления персоны пользователя
/// </summary>
public record UpdateUserPersonCommand(UserPersonDto? Dto) : IRequest<bool>
{
}