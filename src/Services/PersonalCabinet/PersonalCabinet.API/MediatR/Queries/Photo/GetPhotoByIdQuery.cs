using MediatR;

using PersonalCabinet.API.Models.DTO.Photo;

namespace PersonalCabinet.API.MediatR.Queries.Photo;

/// <summary>
/// Запрос на получение загруженного пользователем фото по id
/// </summary>
public record GetPhotoByIdQuery(Guid Id) : IRequest<PhotoDto?>
{
}
