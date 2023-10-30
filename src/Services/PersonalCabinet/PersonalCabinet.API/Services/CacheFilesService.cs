using Microsoft.Extensions.Caching.Distributed;

using PersonalCabinet.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.Extensions;

namespace PersonalCabinet.API.Services;

public class CacheFilesService : ICacheFilesService
{
	// кэш редиса
	private readonly IDistributedCache _cache;

	public CacheFilesService(IDistributedCache cache)
	{
		_cache = cache;
	}

	// в случаи, если данных в кэше нет, то возвращаем объект только с id, чтобы клиент мог понять, что данных нет и запросил бы напрямую из 
	// хранилища файлов

	public IEnumerable<FileDto> GetFilesFromCache(params Guid[] fileIds)
	{
		ArgumentNullException.ThrowIfNull(fileIds, nameof(fileIds));

		var result = new List<FileDto>();

		foreach (var id in fileIds) 
		{
			var flag = _cache.TryGet<FileDto>(id.ToString(), out var fileDto);

			if (!flag)
			{
				result.Add(new FileDto() { Id = id });
				continue;
			}

			result.Add(fileDto);
		}

		return result;
	}

	public FileDto? GetFileFromCacheById(Guid fileId)
	{
		var _ = _cache.TryGet<FileDto>(fileId.ToString(), out var fileDto);

		return fileDto;
	}
}