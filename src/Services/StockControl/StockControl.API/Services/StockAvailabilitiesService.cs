using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;
using Service.Common.Extensions;
using Service.Common.Interfaces;

using StockControl.API.DAL.Context;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Models.DTO.StockAvailability;
using StockControl.API.Services.Interfaces;

namespace StockControl.API.Services;

public class StockAvailabilitiesService : IStockAvailabilitiesService
{
	private readonly StockControlDB _db;
	private readonly ISaveService<StockControlDB> _saveService;
	private readonly ILogger<StockAvailabilitiesService> _logger;

	private readonly Guid _userId;

	public StockAvailabilitiesService(StockControlDB dB, ISaveService<StockControlDB> saveService, ILogger<StockAvailabilitiesService> logger, IIdentityService identityService)
	{
		_db = dB;
		_saveService = saveService;
		_userId = identityService.GetUserIdIdentity() ?? throw new InvalidOperationException($"Пользователь не найден");
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<StockAvailabilityDto>> GetListAsync(StockAvailabilityFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.StockAvailabilities
			.Where(s => !s.DeletedDate.HasValue);

		var search = filter.Search;

		if (!string.IsNullOrEmpty(search))
		{
			query = query.Where(q => q.Nomenclature.Name.Contains(search) || q.Organization.Name.Contains(search) || q.Warehouse.Name.Contains(search));
		}

		if (filter.PartyId.HasValue)
			query = query.Where(q => q.PartyId == filter.PartyId.Value);

		if (filter.OrganizationId.HasValue)
			query = query.Where(q => q.OrganizationId == filter.OrganizationId.Value);

		if (filter.NomenclatureId.HasValue)
			query = query.Where(q => q.NomenclatureId == filter.NomenclatureId.Value);

		if (filter.WarehouseId.HasValue)
			query = query.Where(q => q.WarehouseId == filter.WarehouseId.Value);

		// тк партия уникальная и формируется при поступлении а потом кочует по всем перемещениям и списаниям соответствующей продукции, то
		// могут быть задвоенные номенклатуры, от них надо избавится и суммировать их остаток, если совпадает прайс, если не совпадает то не суммируем
		// цены могут ставится вручную в каждом типе движения товара, при этом не меняя цены других типов движения товара
		var queryGroupBy = query
			.GroupBy(q => new { q.PartyId, q.NomenclatureId, q.WarehouseId, q.OrganizationId, q.Price });

		var totalItems = await queryGroupBy.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<StockAvailabilityDto>();

		if (filter.Order is null || string.IsNullOrEmpty(filter.Order.Direction))
			query = query.OrderBy(q => q.PartyId);
		else
			query = query.OrderByColumn(filter.Order);

		// ещё раз создаём запрос, чтобы применить пайджинг и включить джойны
		queryGroupBy = query
			.Skip(filter.Skip)
			.Take(filter.Take)
			.Include(q => q.Party)
			.Include(q => q.Nomenclature)
			.Include(q => q.Warehouse)
			.Include(q => q.Organization)
			.GroupBy(q => new { q.PartyId, q.NomenclatureId, q.WarehouseId, q.OrganizationId, q.Price });


		var entitiesGroupBy = await queryGroupBy
					.AsNoTracking()
					.ToListAsync()
					.ConfigureAwait(false);

		var dtoItems = new List<StockAvailabilityDto>();

		entitiesGroupBy.ForEach(e =>
		{
			var dtoItem = e.First().CreateDto()!;

			// надо суммировать кол-во остатков по номенклатуре
			dtoItem.Quantity = e.Sum(q => q.Quantity);

			dtoItems.Add(dtoItem);
		});

		return new PaginatedItemsDto<StockAvailabilityDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<StockAvailabilityDto?> GetByIdAsync(Guid id)
	{
		var entity = await _db.StockAvailabilities
			.Include(q => q.Party)
			.Include(q => q.Nomenclature)
			.Include(q => q.Warehouse)
			.Include(q => q.Organization)
			.AsNoTracking()
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Остатков с id: {0} не найдено в БД. Операция чтения невозможна.", id);
			return null;
		}

		return entity.CreateDto();
	}

	public async Task<Guid> CreateAsync(StockAvailabilityDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = dto.CreateEntity(_userId)!;

		await _db.AddAsync(entity).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = entity.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(StockAvailabilityDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = await _db.StockAvailabilities
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == dto.Id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Остатков с id: {0} не найдено в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		entity.UpdateEntity(dto, _userId);

		_db.Update(entity);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = await _db.StockAvailabilities
			.FirstOrDefaultAsync(c => c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Остатков с id: {0} не найдено в БД. Операция удаления невозможна.", id);
			return false;
		}

		entity.DeletedBy = _userId;
		entity.DeletedDate = DateTimeOffset.Now.ToLocalTime();

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<BulkDeleteResultDto?> BulkDeleteAsync(params Guid[] ids)
	{
		ArgumentNullException.ThrowIfNull(ids, nameof(ids));

		var entities = await _db.StockAvailabilities
			.Where(s => ids.Contains(s.Id))
			.Include(s => s.Nomenclature)
			.ToArrayAsync()
			.ConfigureAwait(false);

		if (entities.Length == 0)
		{
			_logger.LogWarning("Остатков с ids: {0} не найдено в БД. Операция массового удаления невозможна.", string.Join(";", ids));
			return new BulkDeleteResultDto()
			{
				ErrorMessage = new List<string>()
				{
					"Остатки не найдены в БД. Операция массового удаления невозможна."
				}
			};
		}

		foreach (var entity in entities)
		{
			entity.DeletedBy = _userId;
			entity.DeletedDate = DateTimeOffset.Now.ToLocalTime();
		}

		await _saveService.SaveAsync(_db);

		return new BulkDeleteResultDto()
		{
			SuccessMessage = new BulkDeleteSuccessMessageDto()
			{
				Message = $"Остатки : {string.Join(",", entities.Select(e => e.Nomenclature.Name))} успешно удалены",
				Ids = entities.Select(s => s.Id)
			}
		};
	}

	public async Task<StockAvailabilityDto?> GetRemainderOfSenderWarehouseAsync(Guid sendingWarehouseId, Guid partyId)
	{
		var entity = await _db.StockAvailabilities
			.Where(s => s.WarehouseId == sendingWarehouseId && s.PartyId == partyId)
			.Where(s => !s.DeletedDate.HasValue)
			.Include(q => q.Party)
			.Include(q => q.Nomenclature)
			.Include(q => q.Warehouse)
			.Include(q => q.Organization)
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);

		if (entity is null)
			return null;

		return entity.CreateDto()!;
	}
}
