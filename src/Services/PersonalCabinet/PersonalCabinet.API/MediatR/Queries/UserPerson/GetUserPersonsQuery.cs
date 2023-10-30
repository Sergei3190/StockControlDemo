using MediatR;

using PersonalCabinet.API.Models.DTO.UserPerson;

using Service.Common.DTO;

namespace PersonalCabinet.API.MediatR.Queries.UserPerson;

/// <summary>
/// Запрос на получение отфильтрованных персон пользователей
/// </summary>
public record GetUserPersonsQuery(UserPersonFilterDto Filter) : IRequest<PaginatedItemsDto<UserPersonDto>>
{
}
