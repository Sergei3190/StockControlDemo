using MediatR;

using PersonalCabinet.API.Models.DTO.Photo;

namespace PersonalCabinet.API.MediatR.Commands.Photo;

/// <summary>
/// Команда обновления фото пользователя
/// </summary>
public record UpdatePhotoCommand(PhotoDto? Dto) : IRequest<bool>
{
}