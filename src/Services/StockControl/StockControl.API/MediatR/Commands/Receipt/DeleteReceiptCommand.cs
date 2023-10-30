using MediatR;

namespace StockControl.API.MediatR.Commands.Receipt;

/// <summary>
/// Команда удаления поступления
/// </summary>
public record DeleteReceiptCommand(Guid Id) : IRequest<bool>
{
}