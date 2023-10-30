using MediatR;
using StockControl.API.Models.DTO.WriteOff;

namespace StockControl.API.MediatR.Queries.WriteOff;

/// <summary>
/// Запрос на получение списания по id
/// </summary>
/// <param name="Id"></param>
public record GetWriteOffByIdQuery(Guid Id) : IRequest<WriteOffDto?>
{
}
