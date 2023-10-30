using MediatR;

using StockControl.API.Models.DTO.Moving;

namespace StockControl.API.MediatR.Queries.Moving;

/// <summary>
/// Запрос на получение перемещения по id
/// </summary>
/// <param name="Id"></param>
public record GetMovingByIdQuery(Guid Id) : IRequest<MovingDto?>
{
}
