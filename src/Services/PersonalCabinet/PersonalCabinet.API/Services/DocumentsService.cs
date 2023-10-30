using Microsoft.EntityFrameworkCore;

using PersonalCabinet.API.DAL.Context;
using PersonalCabinet.API.Domain.Person;
using PersonalCabinet.API.Infrastructure.Mappers;
using PersonalCabinet.API.Models.DTO.Document;
using PersonalCabinet.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.Interfaces;

namespace PersonalCabinet.API.Services;

public class DocumentsService : IDocumentsService
{
	private const string DocumentMnemo = "Document";

	private readonly PersonalCabinetDB _db;
	private readonly ISaveService<PersonalCabinetDB> _saveService;
	private readonly ICacheFilesService _cacheFileService;
	private readonly ILogger<DocumentsService> _logger;

	private readonly Guid _userId;

	public DocumentsService(PersonalCabinetDB dB,
		ISaveService<PersonalCabinetDB> saveService,
		ICacheFilesService cacheFileService,
		ILogger<DocumentsService> logger,
		IIdentityService identityService)
	{
		_db = dB;
		_saveService = saveService;
		_cacheFileService = cacheFileService;
		_userId = identityService.GetUserIdIdentity() ?? throw new InvalidOperationException($"Пользователь не найден");
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<DocumentDto>> GetListAsync(DocumentFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.Documents
			.Where(s => !s.DeletedDate.HasValue)
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.FileName.Contains(filter.Search));
		}

		if (filter.CardId.HasValue)
			query = query.Where(q => q.CardId == filter.CardId.Value);

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<DocumentDto>();

		var entities = await query
			.OrderBy(q => q.Id)
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = new List<DocumentDto>();

		foreach (var entity in entities)
		{
			var dtoItem = entity.CreateDto()!;

			var fileDto = _cacheFileService.GetFileFromCacheById(entity.ExternalId);

			if (fileDto != null)
				dtoItem.File = fileDto;

			dtoItems.Add(dtoItem);
		}

		return new PaginatedItemsDto<DocumentDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<DocumentDto?> GetByIdAsync(Guid id)
	{
		var entity = await _db.Documents
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive)
			.AsNoTracking()
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Документ с id: {0} не найдено в БД. Операция чтения невозможна.", id);
			return null;
		}

		var dto = entity.CreateDto()!;

		// получаем файл из кэша рэдиса
		var fileDto = _cacheFileService.GetFileFromCacheById(entity.ExternalId);

		if (fileDto != null)
			dto.File = fileDto;

		return dto;
	}

	public async Task<Guid> CreateAsync(DocumentDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		if (dto.LoadedDataType is null)
		{
			dto.LoadedDataType = LoadedDataType.LoadedDataTypes
				.First(t => t.Mnemo == DocumentMnemo)
				.CreateDto();
		}

		var entity = dto.CreateEntity(_userId)!;

		await _db.AddAsync(entity).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = entity.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(DocumentDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = await _db.Documents
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive)
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == dto.Id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Документ с id: {0} не найдено в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		entity.UpdateEntity(dto, _userId);

		_db.Update(entity);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = await _db.Documents
			.Include(s => s.LoadedDataType)
			.Where(s => s.LoadedDataType.IsActive)
			.FirstOrDefaultAsync(c => c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Документ с id: {0} не найдено в БД. Операция удаления невозможна.", id);
			return false;
		}

		entity.DeletedBy = _userId;
		entity.DeletedDate = DateTimeOffset.Now.ToLocalTime();

		await _saveService.SaveAsync(_db);

		return true;
	}
}
