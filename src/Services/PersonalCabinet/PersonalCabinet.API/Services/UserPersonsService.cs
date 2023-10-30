using Microsoft.EntityFrameworkCore;

using PersonalCabinet.API.DAL.Context;
using PersonalCabinet.API.Domain.Person;
using PersonalCabinet.API.Infrastructure.Mappers;
using PersonalCabinet.API.Models.DTO.UserPerson;
using PersonalCabinet.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.Interfaces;

namespace PersonalCabinet.API.Services;

public class UserPersonsService : IUserPersonsService
{
	private readonly PersonalCabinetDB _db;
	private readonly ISaveService<PersonalCabinetDB> _saveService;
	private readonly ILogger<UserPersonsService> _logger;

	private readonly Guid? _userId;

	public UserPersonsService(PersonalCabinetDB dB,
		ISaveService<PersonalCabinetDB> saveService,
		ILogger<UserPersonsService> logger,
		IIdentityService identityService)
	{
		_db = dB;
		_saveService = saveService;
		// здесь не нужна жёсткая проверка на пользователя, тк мы создаём персону изначально после получения пользователя через щину
		_userId = identityService.GetUserIdIdentity();
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<UserPersonDto>> GetListAsync(UserPersonFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.UserPersons
			.Where(s => !s.DeletedDate.HasValue);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.LastName.Contains(filter.Search) || q.FirstName.Contains(filter.Search) || q.MiddleName!.Contains(filter.Search));
		}

		if (filter.Age.HasValue)
			query = query.Where(q => q.Age == filter.Age.Value);

		if (filter.Birthday.HasValue)
			query = query.Where(q => q.Birthday == filter.Birthday.Value);

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<UserPersonDto>();

		var entities = await query
			.OrderBy(q => q.LastName)
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<UserPerson>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<UserPersonDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<UserPersonDto?> GetByIdAsync(Guid id)
	{
		var entity = await _db.UserPersons
			.AsNoTracking()
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Персона с id: {0} не найдена в БД. Операция чтения невозможна.", id);
			return null;
		}

		return entity.CreateDto()!;
	}

	//	для одного CardId всегда одно действующее фото, как и для одного персона, а вот для документов у нас вернётся коллекция
	public async Task<UserPersonDto?> GetByCardIdAsync(Guid cardId)
	{
		var entity = await _db.UserPersons
			.AsNoTracking()
			.SingleOrDefaultAsync(c => !c.DeletedDate.HasValue && c.CardId == cardId)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Персона по cardId: {0} не найдена в БД. Операция чтения невозможна.", cardId);
			return null;
		}

		return entity.CreateDto()!;
	}

	public async Task<UserPersonDto?> GetPersonCurrentUserAsync()
	{
		if (_userId is null)
			return null;

		var entity = await _db.UserPersons
			.AsNoTracking()
			.SingleOrDefaultAsync(c => !c.DeletedDate.HasValue && c.UserId == _userId)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Персона по userId: {0} не найдена в БД. Операция чтения невозможна.", _userId);
			return null;
		}

		return entity.CreateDto()!;

	}

	public async Task<Guid> CreateAsync(UserPersonDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = dto.CreateEntity(_userId ?? dto.UserId)!;

		await _db.AddAsync(entity).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = entity.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(UserPersonDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = await _db.UserPersons
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == dto.Id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Персона с id: {0} не найдена в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		entity.UpdateEntity(dto, _userId);

		_db.Update(entity);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = await _db.UserPersons
			.FirstOrDefaultAsync(c => c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Персона с id: {0} не найдена в БД. Операция удаления невозможна.", id);
			return false;
		}

		entity.DeletedBy = _userId ?? throw new InvalidOperationException($"Пользователь не задан");
		entity.DeletedDate = DateTimeOffset.Now.ToLocalTime();

		await _saveService.SaveAsync(_db);

		return true;
	}
}
