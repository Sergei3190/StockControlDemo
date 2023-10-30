using MediatR;

using PersonalCabinet.API.Models.DTO.UserPerson;

namespace PersonalCabinet.API.MediatR.Queries.UserPerson;

/// <summary>
/// Запрос на получение персоны пользователя по cardId
/// </summary>
public record GetUserPersonByCardIdQuery(Guid Id) : IRequest<UserPersonDto?>
{
}
