using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;
using Service.Common.Extensions;
using Service.Common.Interfaces;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Stock;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Models.DTO.Nomenclature;
using StockControl.API.Services.Interfaces.ClassifierItems;

namespace StockControl.API.Services.ClassifierItems;

public class NomenclaturesService : INomenclaturesService
{
	private const string NomenclatureMnemo = "Nomenclature";

	private readonly StockControlDB _db;
	private readonly ISaveService<StockControlDB> _saveService;
	private readonly ILogger<NomenclaturesService> _logger;

	private readonly Guid _userId;

	public NomenclaturesService(StockControlDB dB, ISaveService<StockControlDB> saveService, ILogger<NomenclaturesService> logger, IIdentityService identityService)
	{
		_db = dB;
		_saveService = saveService;
		_userId = identityService.GetUserIdIdentity() ?? throw new InvalidOperationException($"Пользователь не найден");
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<NomenclatureDto>> GetListAsync(NomenclatureFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.Nomenclatures
			.Where(s => !s.DeletedDate.HasValue)
			.Include(s => s.Classifier)
			.Where(s => s.Classifier.IsActive && s.Classifier.Mnemo == NomenclatureMnemo);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.Name.Contains(filter.Search));
		}

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<NomenclatureDto>();

		if (filter.Order is null || string.IsNullOrEmpty(filter.Order.Direction))
			query = query.OrderBy(q => q.Name);
		else
			query = query.OrderByColumn(filter.Order);

		var entities = await query
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<Nomenclature>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<NomenclatureDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	public async Task<NomenclatureDto?> GetByIdAsync(Guid id)
	{
		var entity = await _db.Nomenclatures
			.Include(s => s.Classifier)
			.Where(s => s.Classifier.IsActive)
			.AsNoTracking()
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Номенклатура с id: {0} не найдена в БД. Операция чтения невозможна.", id);
			return null;
		}

		return entity.CreateDto();
	}

	public async Task<Guid> CreateAsync(NomenclatureDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		if (dto.Classifier is null)
		{
			dto.Classifier = Classifier.Classifiers
				.First(t => t.Mnemo == NomenclatureMnemo)
				.CreateDto()!;
		}

		var entity = dto.CreateEntity(_userId)!;

		await _db.AddAsync(entity).ConfigureAwait(false);

		await _saveService.SaveAsync(_db);

		dto.Id = entity.Id;

		return dto.Id;
	}

	public async Task<bool> UpdateAsync(NomenclatureDto? dto)
	{
		ArgumentNullException.ThrowIfNull(dto, nameof(dto));

		var entity = await _db.Nomenclatures
			.Include(s => s.Classifier)
			.Where(s => s.Classifier.IsActive)
			.FirstOrDefaultAsync(c => !c.DeletedDate.HasValue && c.Id == dto.Id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Номенклатура с id: {0} не найдена в БД. Операция обновления невозможна.", dto.Id);
			return false;
		}

		entity.UpdateEntity(dto, _userId);

		_db.Update(entity);

		await _saveService.SaveAsync(_db);

		return true;
	}

	public async Task<bool> DeleteAsync(Guid id)
	{
		var entity = await _db.Nomenclatures
			.Include(s => s.Classifier)
			.Where(s => s.Classifier.IsActive)
			.FirstOrDefaultAsync(c => c.Id == id)
			.ConfigureAwait(false);

		if (entity is null)
		{
			_logger.LogWarning("Номенклатура с id: {0} не найдена в БД. Операция удаления невозможна.", id);
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

		var entities = await _db.Nomenclatures
			.Where(s => ids.Contains(s.Id))
			.ToArrayAsync()
			.ConfigureAwait(false);

		if (entities.Length == 0)
		{
			_logger.LogWarning("Номенклатура с ids: {0} не найдена в БД. Операция массового удаления невозможна.", string.Join(";", ids));
			return new BulkDeleteResultDto()
			{
				ErrorMessage = new List<string>()
				{
					"Номенклатура не найдена в БД. Операция массового удаления невозможна."
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
				Message = $"Номенклатура : {string.Join(",", entities.Select(e => e.Name))} успешно удалена",
				Ids = entities.Select(s => s.Id)
			} 
		};
	}

	public async Task<IEnumerable<(Guid itemId, string name, string number)>> GetProductFlowNumbersByItemIdAsync(params Guid[] ids)
	{
		var result = new List<(Guid itemId, string name, string number)>();

		var receiptNumbers = await GetReceiptNumdersAsync(ids).ConfigureAwait(false);

		if (receiptNumbers.Any())
		{
			_logger.LogInformation("Получены номера поступлений (numbers: {numbers})," +
				" в которых задейстована номенклатура: {ids}", string.Join(",", receiptNumbers.Select(r => r.number)), string.Join(",", ids));

			result.AddRange(receiptNumbers);
			return result;
		}

		var movingNumbers = await GetMovingNumdersAsync(ids).ConfigureAwait(false);

		if (movingNumbers.Any())
		{
			_logger.LogInformation("Получены номера перемещений (numbers: {numbers})," +
				" в которых задейстована номенклатура с ids: {ids}", string.Join(",", movingNumbers.Select(r => r.number)), string.Join(",", ids));

			result.AddRange(movingNumbers);
			return result;
		}

		var writeOffNumbers = await GetWriteOffNumdersAsync(ids).ConfigureAwait(false);

		if (writeOffNumbers.Any())
		{
			_logger.LogInformation("Получены номера списаний (numbers: {numbers})," +
				" в которых задейстована номенклатура с ids: {ids}", string.Join(",", writeOffNumbers.Select(r => r.number)), string.Join(",", ids));

			result.AddRange(writeOffNumbers);
			return result;
		}

		return result;
	}

	private async Task<IEnumerable<(Guid itemId, string name, string number)>> GetReceiptNumdersAsync(params Guid[] ids)
	{
		return await _db.Receipts
			.Where(r => !r.DeletedDate.HasValue)
			.Where(r => r.ProductFlowType.IsActive)
			.Where(r => ids.Contains(r.NomenclatureId))
			.AsNoTracking()
			.Select(r => Tuple.Create(r.NomenclatureId, r.Nomenclature.Name, r.Number).ToValueTuple())
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	private async Task<IEnumerable<(Guid itemId, string name, string number)>> GetMovingNumdersAsync(params Guid[] ids)
	{
		return await _db.Movings
			.Where(r => !r.DeletedDate.HasValue)
			.Where(r => r.ProductFlowType.IsActive)
			.Where(r => ids.Contains(r.NomenclatureId))
			.AsNoTracking()
			.Select(r => Tuple.Create(r.NomenclatureId, r.Nomenclature.Name, r.Number).ToValueTuple())
			.ToArrayAsync()
			.ConfigureAwait(false);
	}

	private async Task<IEnumerable<(Guid itemId, string name, string number)>> GetWriteOffNumdersAsync(params Guid[] ids)
	{
		return await _db.WriteOffs
			.Where(r => !r.DeletedDate.HasValue)
			.Where(r => r.ProductFlowType.IsActive)
			.Where(r => ids.Contains(r.NomenclatureId))
			.AsNoTracking()
			.Select(r => Tuple.Create(r.NomenclatureId, r.Nomenclature.Name, r.Number).ToValueTuple())
			.ToArrayAsync()
			.ConfigureAwait(false);
	}
}
