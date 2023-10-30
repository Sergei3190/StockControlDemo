using MediatR;
using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.MediatR.Queries.Nomenclature;

/// <summary>
/// Запрос на получение элемента справочника номенклатуры по id
/// </summary>
/// <param name="Id"></param>
public record GetNomenclatureByIdQuery(Guid Id) : IRequest<NomenclatureDto?>
{
}
