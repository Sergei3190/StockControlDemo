using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;
using Service.Common.Extensions;
using Service.Common.Interfaces;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Stock;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Models.DTO.Receipt;
using StockControl.API.Services.Interfaces.ProductFlow;

namespace StockControl.API.Services.ProductFlow;

public class ReceiptsService : IReceiptsService
{
	private const string ReceiptMnemo = "Receipt";

	private readonly StockControlDB _db;
	private readonly ISaveService<StockControlDB> _saveService;
	private readonly ILogger<ReceiptsService> _logger;

	private readonly Guid _userId;

	public ReceiptsService(StockControlDB dB, ISaveService<StockControlDB> saveService, ILogger<ReceiptsService> logger, IIdentityService identityService)
	{
		_db = dB;
		_saveService = saveService;
		_userId = identityService.GetUserIdIdentity() ?? throw new InvalidOperationException($"Пользователь не найден");
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<ReceiptDto>> GetListAsync(ReceiptFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.Receipts
			.Where(s => !s.DeletedDate.HasValue)
			.Include(s => s.ProductFlowType)
			.Where(s => s.ProductFlowType.IsActive);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.Number.Contains(filter.Search));
		}

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
			return new PaginatedItemsDto<ReceiptDto>();

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
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<Receipt>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<ReceiptDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<ReceiptDto?> GetByIdAsync(Guid id)
	{
		var entity = await _db.Receipts
			.Include(s => s.ProductFlowType)
			.Where(s => s.ProductFlowType.IsActive)
			.Include(q => q.Party)
			.Include(q => q.Nomenclature)
			.Include(q => q.Warehouse)
			.Include(q => q.Organization)
			.AsNoTracking()
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Поступление с id: {0} не найдено в БД. Операция чтения невозможна.", id);
			return null;
		}

		return entity.CreateDto();
	}

	public async Task<Guid> CreateAsync(ReceiptDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		if (dto.ProductFlowType is null)
		{
			dto.ProductFlowType = ProductFlowType.ProductFlowTypes
				.First(t => t.Mnemo == ReceiptMnemo)
				.CreateDto()!;
		}

		var entity = dto.CreateEntity(_userId)!;

		await _db.AddAsync(entity).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = entity.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(ReceiptDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = await _db.Receipts
			.Include(s => s.ProductFlowType)
			.Where(s => s.ProductFlowType.IsActive)
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == dto.Id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Поступление с id: {0} не найдено в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		entity.UpdateEntity(dto, _userId);

		_db.Update(entity);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = await _db.Receipts
			.Include(s => s.ProductFlowType)
			.Where(s => s.ProductFlowType.IsActive)
			.FirstOrDefaultAsync(c => c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Поступление с id: {0} не найдено в БД. Операция удаления невозможна.", id);
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

		var entities = await _db.Receipts
			.Where(s => ids.Contains(s.Id))
			.ToArrayAsync()
			.ConfigureAwait(false);

		if (entities.Length == 0)
		{
			_logger.LogWarning("Поступлений с ids: {0} не найдено в БД. Операция массового удаления невозможна.", string.Join(";", ids));
			return new BulkDeleteResultDto()
			{
				ErrorMessage = new List<string>()
				{
					"Поступления не найдены в БД. Операция массового удаления невозможна."
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
				Message = $"Поступления : {string.Join(",", entities.Select(e => e.Number))} успешно удалены",
				Ids = entities.Select(s => s.Id)
			}
		};
	}

	public async Task<IEnumerable<(Guid ItemId, int InvolvedQuantity)>> GetInvolvedQuantityAsync(params (Guid Id, Guid PartyId, Guid WarehouseId)[] dtos)
	{
		var partyIds = dtos.Select(d => d.PartyId);
		var warhouseIds = dtos.Select(d => d.WarehouseId);

		// получаем уникальные внутренние партии и общее кол-во задействованной продукции по этим партиям
		var info = await _db.WriteOffs
			.Where(w => partyIds.Contains(w.PartyId) && warhouseIds.Contains(w.SendingWarehouseId!.Value))
			.Where(w => !w.DeletedDate.HasValue)
			.AsNoTracking()
			.Select(w => new { w.Quantity, w.PartyId })
			.Union(_db.Movings
			.Where(m => partyIds.Contains(m.PartyId) && warhouseIds.Contains(m.SendingWarehouseId!.Value))
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
		return await _db.Receipts
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
		return await _db.Receipts
			.Where(x => ids.Contains(x.Id))
			.AsNoTracking()
			.Select(x => Tuple.Create(x.Id, x.Number).ToValueTuple())
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	public async Task<(DateOnly CreateDate, TimeOnly CreateTime)?> GetDataTimeCreatedByPartyIdAsync(Guid partyId)
	{
		return await _db.Receipts
			.Where(r => r.PartyId == partyId)
			.Where(r => !r.DeletedDate.HasValue)
			.AsNoTracking()
			.Select(r => Tuple.Create(r.CreateDate, r.CreateTime).ToValueTuple())
			.FirstOrDefaultAsync()
			.ConfigureAwait(false);
	}
}
