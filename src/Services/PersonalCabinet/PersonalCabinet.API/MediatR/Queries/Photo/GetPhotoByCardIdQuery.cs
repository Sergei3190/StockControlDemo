using MediatR;

using PersonalCabinet.API.Models.DTO.Photo;

namespace PersonalCabinet.API.MediatR.Queries.Photo;

/// <summary>
/// Запрос на получение загруженного пользователем фото по cardId
/// </summary>
public record GetPhotoByCardIdQuery(Guid Id) : IRequest<PhotoDto?>
{
}
