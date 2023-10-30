using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using PersonalCabinet.API.DAL.Context;
using PersonalCabinet.API.Domain.Person;
using PersonalCabinet.API.Infrastructure.Mappers;
using PersonalCabinet.API.Models.DTO.Photo;
using PersonalCabinet.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.Interfaces;

namespace PersonalCabinet.API.Services;

public class PhotosService : IPhotosService
{
	private const string PhotoMnemo = "Photo";

	/// <summary>
	/// Ширина, до которой сжимается фотография (пропорция сохраняется)
	/// </summary>
	private const int PhotoWidth = 170;

	private readonly PersonalCabinetDB _db;
	private readonly ISaveService<PersonalCabinetDB> _saveService;
	private readonly ICacheFilesService _cacheFileService;
	private readonly IMemoryCache _memoryCache;
	private readonly IImagesService _imagesService;
	private readonly ILogger<PhotosService> _logger;

	private readonly Guid _userId;

	public PhotosService(PersonalCabinetDB dB,
		ISaveService<PersonalCabinetDB> saveService,
		ICacheFilesService cacheFileService,
		IMemoryCache memoryCache,
		IImagesService imagesService,
		ILogger<PhotosService> logger,
		IIdentityService identityService)
	{
		_db = dB;
		_saveService = saveService;
		_cacheFileService = cacheFileService;
		_memoryCache = memoryCache;
		_imagesService = imagesService;
		_userId = identityService.GetUserIdIdentity() ?? throw new InvalidOperationException($"Пользователь не найден");
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<PhotoDto>> GetListAsync(PhotoFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.Photos
			.Where(s => !s.DeletedDate.HasValue)
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.FileName.Contains(filter.Search));
		}

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<PhotoDto>();

		var entities = await query
			.OrderBy(q => q.FileName)
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = new List<PhotoDto>();

		foreach (var entity in entities)
		{
			var dtoItem = entity.CreateDto()!;

			var fileDto = GetFileDtoFromCache(entity.ExternalId);

			if (fileDto != null)
				dtoItem.File = fileDto;

			dtoItems.Add(dtoItem);
		}

		return new PaginatedItemsDto<PhotoDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<PhotoDto?> GetByIdAsync(Guid id)
	{
		var entity = await _db.Photos
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive)
			.AsNoTracking()
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Фото с id: {0} не найдено в БД. Операция чтения невозможна.", id);
			return null;
		}

		var dto = entity.CreateDto()!;

		var fileDto = GetFileDtoFromCache(entity.ExternalId);

		if (fileDto != null)
			dto.File = fileDto;

		return dto;
	}

	//	для одного CardId всегда одно действующее фото, как и для одного персона, а вот для документов у нас вернётся коллекция
	public async Task<PhotoDto?> GetByCardIdAsync(Guid cardId)
	{
		var entity = await _db.Photos
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive)
			.AsNoTracking()
			.SingleOrDefaultAsync(c => !c.DeletedDate.HasValue && c.CardId == cardId)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Фото по cardId: {0} не найдено в БД. Операция чтения невозможна.", cardId);
			return null;
		}

		var dto = entity.CreateDto()!;

		var fileDto = GetFileDtoFromCache(entity.ExternalId);

		if (fileDto != null)
			dto.File = fileDto;

		return dto;
	}

	public async Task<Guid> CreateAsync(PhotoDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		if (dto.LoadedDataType is null)
		{
			dto.LoadedDataType = LoadedDataType.LoadedDataTypes
				.First(t => t.Mnemo == PhotoMnemo)
				.CreateDto();
		}

		var entity = dto.CreateEntity(_userId)!;

		await _db.AddAsync(entity).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = entity.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(PhotoDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = await _db.Photos
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive)
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == dto.Id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Фото с id: {0} не найдено в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		entity.UpdateEntity(dto, _userId);

		_db.Update(entity);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = await _db.Photos
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive)
			.FirstOrDefaultAsync(c => c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Фото с id: {0} не найдено в БД. Операция удаления невозможна.", id);
			return false;
		}

		entity.DeletedBy = _userId;
		entity.DeletedDate = DateTimeOffset.Now.ToLocalTime();

		await _saveService.SaveAsync(_db);

		return true;
	}

	/// <summary>
	/// Получаем фото из кэша
	/// </summary>
	private FileDto? GetFileDtoFromCache(Guid id)
	{
		var fileDto = _cacheFileService.GetFileFromCacheById(id);

		if (fileDto is null)
			return null;

		var result = _memoryCache.TryGetValue(fileDto.Id, out byte[]? photo);

		if (!result)
		{
			photo = GetPhoto(fileDto.Content!);
			_memoryCache.Set(fileDto.Id, photo);
		}

		fileDto.Content = photo!;

		return fileDto;
	}

	/// <summary>
	/// Получить фото
	/// </summary>
	private byte[] GetPhoto(byte[] content)
	{
		var image = _imagesService.ConvertByteArrayToImage(content);
		return _imagesService.ConvertImageToByteArray(image)!;
	}
}
