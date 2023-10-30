using MediatR;

using Service.Common.DTO;

using StockControl.API.Models.DTO.Classifier;

namespace StockControl.API.MediatR.Queries.Select;

/// <summary>
/// Запрос на получение отфильтрованных справочников
/// </summary>
public record GetClassifiersQuery(ClassifierFilterDto Filter) : IRequest<PaginatedItemsDto<ClassifierDto>>
{
}
