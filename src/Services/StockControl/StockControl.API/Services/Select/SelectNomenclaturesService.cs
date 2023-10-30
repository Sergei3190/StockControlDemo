using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Stock;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Models.DTO.Nomenclature;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.Services.Select;

public class SelectNomenclaturesService : ISelectNomenclaturesService
{
	private readonly StockControlDB _db;
	private readonly ILogger<SelectNomenclaturesService> _logger;

	public SelectNomenclaturesService(StockControlDB dB, ILogger<SelectNomenclaturesService> logger)
	{
		_db = dB;
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> SelectAsync(SelectNomenclatureFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.Nomenclatures
			.Where(s => !s.DeletedDate.HasValue)
			.Include(s => s.Classifier)
			.Where(s => s.Classifier.IsActive);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.Name.Contains(filter.Search));
		}

		// при создании документа списания, перемещения данные в выпадающий список элементов справочника будут попадать в 
		// зависимости от уже выбранных пользователем данных в используемой форме.
		// при реализации формы фильтра вышеуказанные ограничения не применяются.
		query = SetFilter((filter.OrganizationId, filter.WarehouseId, filter.PartyId), query);

		// группируем, тк если у нас несколько джойнов с одной и той же таблицей, то будут дубли элементов справочников
		query = query
			.OrderBy(q => q.Name)
			.GroupBy(q => q.Id).Select(q => q.First());

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<NamedEntityDto>();

		var entities = await query
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<Nomenclature>().Select(u => u.CreateSelectDto()!);

		return new PaginatedItemsDto<NamedEntityDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	private IQueryable<Nomenclature> SetFilter((Guid? organizationId, Guid? warehouseId, Guid? partyId) data, IQueryable<Nomenclature> query)
	{
		if (data.organizationId.HasValue)
		{
			query = query
				.Join(_db.StockAvailabilities.Where(
					s => !s.DeletedDate.HasValue && s.OrganizationId == data.organizationId.Value),
					c => c.Id,
					s => s.NomenclatureId,
					(c, s) => c);
		}

		if (data.warehouseId.HasValue)
		{
			query = query
				.Join(_db.StockAvailabilities.Where(
					s => !s.DeletedDate.HasValue && s.WarehouseId == data.warehouseId.Value),
					c => c.Id,
					s => s.NomenclatureId,
					(c, s) => c);
		}

		if (data.partyId.HasValue)
		{
			query = query
				.Join(_db.StockAvailabilities.Where(
					s => !s.DeletedDate.HasValue && s.PartyId == data.partyId.Value),
					c => c.Id,
					s => s.NomenclatureId,
					(c, s) => c);
		}

		return query;
	}
}
