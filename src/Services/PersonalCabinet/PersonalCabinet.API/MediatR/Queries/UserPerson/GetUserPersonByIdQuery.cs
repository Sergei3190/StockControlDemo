using MediatR;

using PersonalCabinet.API.Models.DTO.UserPerson;

namespace PersonalCabinet.API.MediatR.Queries.UserPerson;

/// <summary>
/// Запрос на получение персоны пользователя по id
/// </summary>
public record GetUserPersonByIdQuery(Guid Id) : IRequest<UserPersonDto?>
{
}
