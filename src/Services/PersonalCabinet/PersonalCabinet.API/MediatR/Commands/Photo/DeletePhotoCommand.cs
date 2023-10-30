using MediatR;

namespace PersonalCabinet.API.MediatR.Commands.Photo;

/// <summary>
/// Команда удаления фото пользователя
/// </summary>
public record DeletePhotoCommand(Guid Id) : IRequest<bool>
{
}