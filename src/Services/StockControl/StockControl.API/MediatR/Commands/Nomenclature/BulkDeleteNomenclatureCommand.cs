using MediatR;

using Service.Common.DTO;

namespace StockControl.API.MediatR.Commands.Nomenclature;

/// <summary>
/// Команда массового удаления номенклатуры
/// </summary>
public record BulkDeleteNomenclatureCommand(params Guid[] Ids) : IRequest<BulkDeleteResultDto>
{
}