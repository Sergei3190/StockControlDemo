using MediatR;

using Service.Common.DTO;
using StockControl.API.Models.DTO.Moving;

namespace StockControl.API.MediatR.Queries.Moving;

/// <summary>
/// Запрос на получение отфильтрованного списка перемещений
/// </summary>
public record GetMovingsQuery(MovingFilterDto Filter) : IRequest<PaginatedItemsDto<MovingDto>>
{
}
