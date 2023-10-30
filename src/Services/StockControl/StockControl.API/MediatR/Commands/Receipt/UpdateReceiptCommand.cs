using MediatR;

using StockControl.API.Models.DTO.Receipt;

namespace StockControl.API.MediatR.Commands.Receipt;

/// <summary>
/// Команда обновления поступления
/// </summary>
public record UpdateReceiptCommand(ReceiptDto? Dto) : IRequest<bool>
{
}