using FileStorage.API.Infrastructure.Mappers;
using FileStorage.API.MediatR.Queries;
using FileStorage.API.Models;
using FileStorage.API.Services.Interfaces;

using MediatR;

using Microsoft.Extensions.Caching.Distributed;

using Service.Common.DTO;
using Service.Common.Extensions;

namespace FileStorage.API.MediatR.Handlers.QueryHandlers;

public class DownloadFileByIdHandler : IRequestHandler<DownloadFileByIdQuery, FileModel?>
{
	// тк при сохранении файла в монго, в редис записывается FileDto, который может понадобится какому-либо микросервису,
	// который имеет в своей БД объект, у которого имеется id файла в монго, и которому может понадобится информация об этом файле в
	// интерфейсе пользователя без дополнительного обращения к файловому хранилищу (например при инициализации такого объекта в интерфейсе пользователя(фото)),
	// то при добавлении в редис FileModel файлового хранилища, мы будем использовать префикс FS.
	private const string Prefix = "FS";

	private readonly IFileStorageService _filesService;
	// кэш редиса
	private readonly IDistributedCache _cache;

	public DownloadFileByIdHandler(IFileStorageService filesService, IDistributedCache cache)
	{
		_filesService = filesService;
		_cache = cache;
	}

	public async Task<FileModel?> Handle(DownloadFileByIdQuery request, CancellationToken cancellationToken)
	{
		var key = $"{Prefix} {request.Id}";

		var result = _cache.TryGet<FileModel>(key, out var file);

		if (!result)
		{
			file = await _filesService.DownloadByIdAsync(request.Id).ConfigureAwait(false);

			if (file != null)
			{
				await _cache.SetDataAsync(key, file, new DistributedCacheEntryOptions()
				{
					// установим как рабочий день + обед
					AbsoluteExpiration = DateTimeOffset.UtcNow.ToLocalTime().AddHours(9)
				})
					.ConfigureAwait(false);
			}
		}

		// сделаем проверку объекта FileDto в кэше, на случай, если из кэша он был удалён
		await CheckFileDtoInRedis(request.Id, file).ConfigureAwait(false);

		return file;
	}

	private async Task CheckFileDtoInRedis(Guid id, FileModel file)
	{
		var result = _cache.TryGet<FileDto>(id.ToString(), out var fileDto);

		if (!result)
		{
			fileDto = file.CreateFileDto()!;

			await _cache.SetDataAsync(fileDto.Id.ToString(), fileDto, new DistributedCacheEntryOptions()
			{
				// возвращает или задает время, в течение которого запись кэша может быть неактивной (то есть к ней нет обращений), прежде чем она будет удалена.
				SlidingExpiration = TimeSpan.FromDays(30)
			})
			  .ConfigureAwait(false);
		}

		file.File = fileDto;
	}
}