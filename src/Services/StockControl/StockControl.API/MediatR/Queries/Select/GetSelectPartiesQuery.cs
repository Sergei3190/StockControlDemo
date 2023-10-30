using MediatR;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.DTO;

namespace StockControl.API.MediatR.Queries.Select;

/// <summary>
/// Запрос на получение отфильтрованных партий товара
/// </summary>
public record GetSelectPartiesQuery(SelectPartyFilterDto Filter) : IRequest<PaginatedItemsDto<NamedEntityDto>>
{
}
