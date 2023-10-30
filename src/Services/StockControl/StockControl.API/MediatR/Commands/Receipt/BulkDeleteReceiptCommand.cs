using MediatR;

using Service.Common.DTO;

namespace StockControl.API.MediatR.Commands.Receipt;

/// <summary>
/// Команда массового удаления поступлений
/// </summary>
public record BulkDeleteReceiptCommand(params Guid[] Ids) : IRequest<BulkDeleteResultDto>
{
}