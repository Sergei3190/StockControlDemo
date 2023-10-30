using MediatR;

using PersonalCabinet.API.Models.DTO.UserPerson;

namespace PersonalCabinet.API.MediatR.Queries.UserPerson;

/// <summary>
/// Запрос на получение персоны пользователя для текущего пользователя
/// </summary>
public record GetUserPersonCurrentUserQuery() : IRequest<UserPersonDto?>
{
}
