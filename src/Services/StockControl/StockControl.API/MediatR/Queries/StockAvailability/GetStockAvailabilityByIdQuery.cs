using MediatR;

using StockControl.API.Models.DTO.StockAvailability;

namespace StockControl.API.MediatR.Queries.StockAvailability;

/// <summary>
/// Запрос на получение остатка по id
/// </summary>
/// <param name="Id"></param>
public record GetStockAvailabilityByIdQuery(Guid Id) : IRequest<StockAvailabilityDto?>
{
}
