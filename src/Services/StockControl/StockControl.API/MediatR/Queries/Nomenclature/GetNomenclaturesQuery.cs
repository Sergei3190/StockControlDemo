using MediatR;

using Service.Common.DTO;
using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.MediatR.Queries.Nomenclature;

/// <summary>
/// Запрос на получение отфильтрованного элемента справочника номенклатуры
/// </summary>
public record GetNomenclaturesQuery(NomenclatureFilterDto Filter) : IRequest<PaginatedItemsDto<NomenclatureDto>>
{
}
