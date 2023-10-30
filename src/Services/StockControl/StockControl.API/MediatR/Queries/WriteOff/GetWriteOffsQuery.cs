using MediatR;

using Service.Common.DTO;
using StockControl.API.Models.DTO.WriteOff;

namespace StockControl.API.MediatR.Queries.WriteOff;

/// <summary>
/// Запрос на получение отфильтрованного списка списаний
/// </summary>
public record GetWriteOffsQuery(WriteOffFilterDto Filter) : IRequest<PaginatedItemsDto<WriteOffDto>>
{
}
