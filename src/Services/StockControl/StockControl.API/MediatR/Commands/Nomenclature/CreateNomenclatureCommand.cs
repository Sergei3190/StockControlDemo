using MediatR;
using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.MediatR.Commands.Nomenclature;

/// <summary>
/// Команда создания номенклатуры
/// </summary>
public record CreateNomenclatureCommand(NomenclatureDto? Dto) : IRequest<Guid>
{
}