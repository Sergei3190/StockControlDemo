using MediatR;

namespace FileStorage.API.Models.Events;

/// <summary>
/// Доменное событие загрузки файла
/// </summary>
public record StorageFileInfoUploadDomainEvent(Guid FileId) : INotification
{
}
