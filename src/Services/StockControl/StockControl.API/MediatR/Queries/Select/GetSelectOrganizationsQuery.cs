using MediatR;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.MediatR.Queries.Select;

/// <summary>
/// Запрос на получение отфильтрованных элементов справочника организации для выпадающего списка
/// </summary>
public record GetSelectOrganizationsQuery(SelectOrganizationFilterDto Filter) : IRequest<PaginatedItemsDto<NamedEntityDto>>
{
}
