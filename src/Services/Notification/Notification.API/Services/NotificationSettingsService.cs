using Microsoft.EntityFrameworkCore;

using Notification.API.DAL.Context;
using Notification.API.Domain.Notice;
using Notification.API.Infrastructure.Mappers;
using Notification.API.Models.DTO.NotificationSetting;
using Notification.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.Interfaces;

namespace Notification.API.Services;

public class NotificationSettingsService : INotificationSettingsService
{
	private readonly NotificationDB _db;
	private readonly ISaveService<NotificationDB> _saveService;
	private readonly ILogger<NotificationSettingsService> _logger;

	private readonly Guid _userId;

	public NotificationSettingsService(NotificationDB dB, ISaveService<NotificationDB> saveService, IIdentityService identityService, ILogger<NotificationSettingsService> logger)
	{
		_db = dB;
		_saveService = saveService;
		_userId = identityService.GetUserIdIdentity() ?? throw new InvalidOperationException($"Пользователь не найден");
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<NotificationSettingDto>> GetListAsync(NotificationSettingFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		// настройки уведомлений получаем по текущему пользователю, если нужен будет интерфейс ля админа, который должен получать настройки
		// всех пользователей, то нужно будет делать доработку
		var query = _db.NotificationSettings
			.Where(u => u.UserId == _userId)
			.Where(u => !u.DeletedDate.HasValue)
			.Include(u => u.NotificationType)
			.AsQueryable();

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.NotificationType.Name.Contains(filter.Search));
		}

		if (filter.NotificationTypeId.HasValue)
			query = query.Where(q => q.NotificationType.Id == filter.NotificationTypeId.Value);

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<NotificationSettingDto>();

		var entities = await query
			.OrderByDescending(q => q.Enable)
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<NotificationSetting>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<NotificationSettingDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<NotificationSettingDto?> GetByIdAsync(Guid id)
	{
		var entity = await _db.NotificationSettings
			.Where(u => u.UserId == _userId)
			.Include(u => u.NotificationType)
			.AsNoTracking()
			.SingleOrDefaultAsync(n => !n.DeletedDate.HasValue && n.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Настройка уведомления с id: {0} не найдена в БД. Операция чтения невозможна.", id);
			return null;
		}

		return entity.CreateDto();
	}

	public async Task<Guid> CreateAsync(NotificationSettingDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = dto.CreateEntity(_userId)!;

		await _db.AddAsync(entity).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = entity.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(NotificationSettingDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = await _db.NotificationSettings
			.Where(u => u.UserId == _userId)
			.SingleOrDefaultAsync(n => !n.DeletedDate.HasValue && n.Id == dto.Id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Настройка уведомления с id: {0} не найдена в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		entity.UpdateEntity(dto, _userId);

		_db.Update(entity);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = await _db.NotificationSettings
			.Where(u => u.UserId == _userId)
			.SingleOrDefaultAsync(x => x.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Настройка уведомления с id: {0} не найдена в БД. Операция удаления невозможна.", id);
			return false;
		}

		entity.DeletedBy = _userId;
		entity.DeletedDate = DateTimeOffset.Now.ToLocalTime();

		await _saveService.SaveAsync(_db);

		return true;
	}
}
