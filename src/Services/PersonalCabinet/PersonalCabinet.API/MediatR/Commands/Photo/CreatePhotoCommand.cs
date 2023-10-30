using MediatR;

using PersonalCabinet.API.Models.DTO.Photo;

namespace PersonalCabinet.API.MediatR.Commands.Photo;

/// <summary>
/// Команда создания фото пользователя
/// </summary>
public record CreatePhotoCommand(PhotoDto? Dto) : IRequest<Guid>
{
}