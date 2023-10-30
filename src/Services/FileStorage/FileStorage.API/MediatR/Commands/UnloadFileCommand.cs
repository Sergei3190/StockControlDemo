using FileStorage.API.Services;

using MediatR;

namespace FileStorage.API.MediatR.Commands;

/// <summary>
/// Команда загрузки файла
/// </summary>
public record UnloadFileCommand(StorageFileInfo FileInfo, Stream Content) : IRequest<Guid>
{
}