using MediatR;

using Service.Common.DTO;

using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.MediatR.Queries.Organization;

/// <summary>
/// Запрос на получение отфильтрованного элемента справочника организации
/// </summary>
public record GetOrganizationsQuery(OrganizationFilterDto Filter) : IRequest<PaginatedItemsDto<OrganizationDto>>
{
}
