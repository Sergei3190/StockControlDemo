using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;
using Service.Common.Extensions;
using Service.Common.Interfaces;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Stock;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Models.DTO.Moving;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.Services.ProductFlow;

public class MovingsService : IMovingsService
{
	private const string MovingMnemo = "Moving";

	private readonly StockControlDB _db;
	private readonly ISaveService<StockControlDB> _saveService;
	private readonly ILogger<MovingsService> _logger;

	private readonly Guid _userId;

	public MovingsService(StockControlDB dB, ISaveService<StockControlDB> saveService, ILogger<MovingsService> logger, IIdentityService identityService)
	{
		_db = dB;
		_saveService = saveService;
		_userId = identityService.GetUserIdIdentity() ?? throw new InvalidOperationException($"Пользователь не найден");
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<MovingDto>> GetListAsync(MovingFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.Movings
			.Where(s => !s.DeletedDate.HasValue)
			.Include(s => s.ProductFlowType)
			.Where(s => s.ProductFlowType.IsActive);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.Number.Contains(filter.Search));
		}

		if (filter.SendingWarehouseId.HasValue)
			query = query.Where(q => q.SendingWarehouseId == filter.SendingWarehouseId.Value);

		if (filter.PartyId.HasValue)
			query = query.Where(q => q.PartyId == filter.PartyId.Value);

		if (filter.OrganizationId.HasValue)
			query = query.Where(q => q.OrganizationId == filter.OrganizationId.Value);

		if (filter.NomenclatureId.HasValue)
			query = query.Where(q => q.NomenclatureId == filter.NomenclatureId.Value);

		if (filter.WarehouseId.HasValue)
			query = query.Where(q => q.WarehouseId == filter.WarehouseId.Value);

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<MovingDto>();

		if (filter.Order is null || string.IsNullOrEmpty(filter.Order.Direction))
			query = query.OrderBy(q => q.Number);
		else
			query = query.OrderByColumn(filter.Order);

		var entities = await query
			.Skip(filter.Skip)
			.Take(filter.Take)
			.Include(q => q.Party)
			.Include(q => q.Nomenclature)
			.Include(q => q.Warehouse)
			.Include(q => q.Organization)
			.Include(q => q.SendingWarehouse)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<Moving>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<MovingDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<MovingDto?> GetByIdAsync(Guid id)
	{
		var entity = await _db.Movings
			.Include(s => s.ProductFlowType)
			.Where(s => s.ProductFlowType.IsActive)
			.Include(q => q.Party)
			.Include(q => q.Nomenclature)
			.Include(q => q.Warehouse)
			.Include(q => q.Organization)
			.Include(q => q.SendingWarehouse)
			.AsNoTracking()
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Перемещение с id: {0} не найдено в БД. Операция чтения невозможна.", id);
			return null;
		}

		return entity.CreateDto();
	}

	public async Task<Guid> CreateAsync(MovingDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		if (dto.SendingWarehouse is null)
			throw new ArgumentNullException("Не заполнен склад отправитель. Создание перемещения невозможно.", nameof(dto.SendingWarehouse));

		if (dto.SendingWarehouse.Id == dto.Warehouse.Id)
			throw new ArgumentNullException("Склад отправитель не может быть равен складу получателю. Создание перемещения невозможно.");

		if (dto.ProductFlowType is null)
		{
			dto.ProductFlowType = ProductFlowType.ProductFlowTypes
				.First(t => t.Mnemo == MovingMnemo)
				.CreateDto()!;
		}

		var entity = dto.CreateEntity(_userId)!;

		await _db.AddAsync(entity).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = entity.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(MovingDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = await _db.Movings
			.Include(s => s.ProductFlowType)
			.Where(s => s.ProductFlowType.IsActive)
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == dto.Id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Перемещение с id: {0} не найдено в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		entity.UpdateEntity(dto, _userId);

		_db.Update(entity);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = await _db.Movings
			.Include(s => s.ProductFlowType)
			.Where(s => s.ProductFlowType.IsActive)
			.FirstOrDefaultAsync(c => c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Перемещение с id: {0} не найдено в БД. Операция удаления невозможна.", id);
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

		var entities = await _db.Movings
			.Where(s => ids.Contains(s.Id))
			.ToArrayAsync()
			.ConfigureAwait(false);

		if (entities.Length == 0)
		{
			_logger.LogWarning("Перемещений с ids: {0} не найдено в БД. Операция массового удаления невозможна.", string.Join(";", ids));
			return new BulkDeleteResultDto()
			{
				ErrorMessage = new List<string>()
				{
					"Перемещения не найдены в БД. Операция массового удаления невозможна."
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
				Message = $"Перемещения : {string.Join(",", entities.Select(e => e.Number))} успешно удалены",
				Ids = entities.Select(s => s.Id)
			}
		};
	}

	public async Task<IEnumerable<(Guid ItemId, int InvolvedQuantity)>> GetInvolvedQuantityAsync(params (Guid Id, Guid PartyId, Guid WarehouseId)[] dtos)
	{
		var ids = dtos.Select(d => d.Id);
		var partyIds = dtos.Select(d => d.PartyId);
		var warhouseIds = dtos.Select(d => d.WarehouseId);

		// получаем уникальные партии и общее кол-во задействованной продукции по этим партиям
		var info = await _db.WriteOffs
			.Where(w => partyIds.Contains(w.PartyId) && warhouseIds.Contains(w.SendingWarehouseId!.Value))
			.Where(w => !w.DeletedDate.HasValue)
			.AsNoTracking()
			.Select(w => new { w.Quantity, w.PartyId })
			.Union(_db.Movings
			.Where(m => !ids.Contains(m.Id) && partyIds.Contains(m.PartyId) && warhouseIds.Contains(m.SendingWarehouseId!.Value))
			.Where(m => !m.DeletedDate.HasValue)
			.AsNoTracking()
			.Select(m => new { m.Quantity, m.PartyId }))
			.GroupBy(r => r.PartyId)
			.Select(g => new { PartyId = g.Key, TotalQuantity = g.Sum(i => i.Quantity) })
			.ToListAsync()
			.ConfigureAwait(false);

		var result = new List<(Guid ItemId, int InvolvedQuantity)>();

		info.ForEach(i =>
		{
			var itemId = dtos.First(d => d.PartyId == i.PartyId).Id;
			result.Add((itemId, i.TotalQuantity));
		});

		return result;
	}

	public async Task<IEnumerable<(Guid Id, Guid PartyId, string Number, Guid WarehouseId)>> GetCheckingDataAsync(params Guid[] ids)
	{
		return await _db.Movings
			.Where(x => ids.Contains(x.Id))
			.Where(x => !x.DeletedDate.HasValue)
			.AsNoTracking()
			.Select(x => Tuple.Create(x.Id, x.PartyId, x.Number, x.WarehouseId).ToValueTuple())
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task<IEnumerable<(Guid Id, string Number)>> GetIntegrationData(params Guid[] ids)
	{
		// здесь могут быть и удаленные, тк мы отправляем интеграционные события и об удалении
		return await _db.Movings
			.Where(x => ids.Contains(x.Id))
			.AsNoTracking()
			.Select(x => Tuple.Create(x.Id, x.Number).ToValueTuple())
			.ToArrayAsync()
			.ConfigureAwait(false);
	}
}
