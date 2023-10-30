using MediatR;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.MediatR.Queries.Select;

/// <summary>
/// Запрос на получение отфильтрованных элементов справочника номенклатура для выпадающего списка
/// </summary>
public record GetSelectNomenclaturesQuery(SelectNomenclatureFilterDto Filter) : IRequest<PaginatedItemsDto<NamedEntityDto>>
{
}
