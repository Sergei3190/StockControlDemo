using FileStorage.API.Models;

namespace FileStorage.API.Services.Interfaces;

/// <summary>
/// Сервис файлового хранилища
/// </summary>
public interface IFileStorageService
{
    Task<Guid> UnloadAsync(StorageFileInfo? fileInfo, Stream? content);

    Task<FileModel?> DownloadByIdAsync(Guid id);
}