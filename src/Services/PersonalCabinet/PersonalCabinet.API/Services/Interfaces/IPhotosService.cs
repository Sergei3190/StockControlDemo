using PersonalCabinet.API.Models.DTO.Photo;

using Service.Common.Interfaces;

namespace PersonalCabinet.API.Services.Interfaces;

/// <summary>
/// Сервис загруженных фото пользователей
/// </summary>
public interface IPhotosService : ICrudService<PhotoDto, PhotoFilterDto>, ICardService<PhotoDto>
{

}