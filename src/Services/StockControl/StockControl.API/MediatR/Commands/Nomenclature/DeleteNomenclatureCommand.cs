using MediatR;

namespace StockControl.API.MediatR.Commands.Nomenclature;

/// <summary>
/// Команда удаления номенклатуры
/// </summary>
public record DeleteNomenclatureCommand(Guid Id) : IRequest<bool>
{
}