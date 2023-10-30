using MediatR;

using PersonalCabinet.API.Models.DTO.LoadedDataType;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.MediatR.Queries.Select;

/// <summary>
/// Запрос на получение типов загруженной информации о пользователе
/// </summary>
public record GetLoadedDataTypesQuery(LoadedDataTypeFilterDto Filter) : IRequest<PaginatedItemsDto<NamedEntityDto>>
{
}
