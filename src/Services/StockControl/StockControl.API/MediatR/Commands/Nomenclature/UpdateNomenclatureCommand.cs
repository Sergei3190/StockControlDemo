using MediatR;
using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.MediatR.Commands.Nomenclature;

/// <summary>
/// Команда обновления номенклатуры
/// </summary>
public record UpdateNomenclatureCommand(NomenclatureDto? Dto) : IRequest<bool>
{
}