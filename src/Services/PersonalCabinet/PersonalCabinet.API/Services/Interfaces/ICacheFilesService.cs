using Service.Common.DTO;

namespace PersonalCabinet.API.Services.Interfaces;

/// <summary>
/// Сервис получения файла из кэша рэдиса
/// </summary>
public interface ICacheFilesService
{
	IEnumerable<FileDto> GetFilesFromCache(params Guid[] fileIds);
	FileDto? GetFileFromCacheById(Guid fileId);
}
