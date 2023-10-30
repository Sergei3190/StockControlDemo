using FileStorage.API.Models;

using MediatR;

namespace FileStorage.API.MediatR.Queries;

/// <summary>
/// Запрос на загрузку файла по id
/// </summary>
public record DownloadFileByIdQuery(Guid Id) : IRequest<FileModel?>
{
}