using FileStorage.API.Infrastructure.Mappers;
using FileStorage.API.Models.Events;
using FileStorage.API.Services.Interfaces;

using MediatR;

using Microsoft.Extensions.Caching.Distributed;

using Service.Common.Extensions;

namespace FileStorage.API.MediatR.Handlers.DomainEventHandlers.StorageFileInfoUpload;

public class SaveFileDtoToRedisHandler : INotificationHandler<StorageFileInfoUploadDomainEvent>
{
	// кэш редиса
	private readonly IDistributedCache _cache;
	private readonly IFileStorageService _filesService;
	private readonly ILogger<SaveFileDtoToRedisHandler> _logger;

	public SaveFileDtoToRedisHandler(IDistributedCache cache, IFileStorageService filesService, ILogger<SaveFileDtoToRedisHandler> logger)
	{
		_cache = cache;
		_filesService = filesService;
		_logger = logger;
	}

	public async Task Handle(StorageFileInfoUploadDomainEvent @event, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(@event, nameof(@event));

		// при сохранении файла в монго, в редис записывается FileDto, который может понадобится какому-либо микросервису,
		// который имеет в своей БД объект, у которого имеется id файла в монго, и которому может понадобится информация об этом файле в
		// интерфейсе пользователя без дополнительного обращения к файловому хранилищу (например при инициализации такого объекта в интерфейсе пользователя (фото)),
		// для скачивания файла всё равно нужно обращаться к файловому хранилищу
		var file = await _filesService.DownloadByIdAsync(@event.FileId).ConfigureAwait(false);

		if (file is null)
		{
			var error = string.Format("Отсутствуют данные для выполнения команды {0}", typeof(File));
			throw new ArgumentNullException(error, nameof(file));
		}

		var fileDto = file.CreateFileDto()!;

		await _cache.SetDataAsync(fileDto.Id.ToString(), fileDto, new DistributedCacheEntryOptions()
		{
			// возвращает или задает время, в течение которого запись кэша может быть неактивной (то есть к ней нет обращений), прежде чем она будет удалена.
			SlidingExpiration = TimeSpan.FromDays(30)
		})
		  .ConfigureAwait(false);
	}
}