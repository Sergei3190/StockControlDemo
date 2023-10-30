using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Stock;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Models.DTO;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.Services.Select;

public class SelectPartiesService : ISelectPartiesService
{
	private readonly StockControlDB _db;
	private readonly ILogger<SelectPartiesService> _logger;

	public SelectPartiesService(StockControlDB dB, ILogger<SelectPartiesService> logger)
	{
		_db = dB;
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> SelectAsync(SelectPartyFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		// партии формируются при поступлении продукции
		var query = _db.Parties
			.Where(p => !p.DeletedDate.HasValue);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.Number.Contains(filter.Search));
		}

		// при создании документа списания, перемещения данные в выпадающий список элементов справочника будут попадать в 
		// зависимости от уже выбранных пользователем данных в используемой форме.
		// при реализации формы фильтра вышеуказанные ограничения не применяются.
		query = SetFilter((filter.NomenclatureId, filter.WarehouseId, filter.OrganizationId), query);

		// группируем, тк если у нас несколько джойнов с одной и той же таблицей, то будут дубли элементов справочников
		query = query
			.OrderBy(q => q.Number)
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

		var dtoItems = entities.Cast<Party>().Select(u => u.CreateSelectDto()!);

		return new PaginatedItemsDto<NamedEntityDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}

	private IQueryable<Party> SetFilter((Guid? nomenclatureId, Guid? warehouseId, Guid? organizationId) data, IQueryable<Party> query)
	{
		if (data.nomenclatureId.HasValue)
		{
			query = query
				.Join(_db.StockAvailabilities.Where(
					s => !s.DeletedDate.HasValue && s.NomenclatureId == data.nomenclatureId.Value),
					c => c.Id,
					s => s.PartyId,
					(c, s) => c);
		}

		if (data.warehouseId.HasValue)
		{
			query = query
				.Join(_db.StockAvailabilities.Where(
					s => !s.DeletedDate.HasValue && s.WarehouseId == data.warehouseId.Value),
					c => c.Id,
					s => s.PartyId,
					(c, s) => c);
		}

		if (data.organizationId.HasValue)
		{
			query = query
				.Join(_db.StockAvailabilities.Where(
					s => !s.DeletedDate.HasValue && s.OrganizationId == data.organizationId.Value),
					c => c.Id,
					s => s.PartyId,
					(c, s) => c);
		}

		return query;
	}
}
