using MediatR;

using StockControl.API.Models.DTO.Receipt;

namespace StockControl.API.MediatR.Commands.Receipt;

/// <summary>
/// Команда создания поступления
/// </summary>
public record CreateReceiptCommand(ReceiptDto? Dto) : IRequest<Guid>
{
}