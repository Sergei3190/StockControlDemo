using MediatR;

using PersonalCabinet.API.Models.DTO.Photo;

using Service.Common.DTO;

namespace PersonalCabinet.API.MediatR.Queries.Photo;

/// <summary>
/// Запрос на получение отфильтрованных загруженных пользователем фото
/// </summary>
public record GetPhotosQuery(PhotoFilterDto Filter) : IRequest<PaginatedItemsDto<PhotoDto>>
{
}
