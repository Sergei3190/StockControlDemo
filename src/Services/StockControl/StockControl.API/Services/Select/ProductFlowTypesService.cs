using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Stock;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Models.DTO.ProductFlowType;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.Services.Select;

public class ProductFlowTypesService : IProductFlowTypesService
{
	private readonly StockControlDB _db;
	private readonly ILogger<ProductFlowTypesService> _logger;

	public ProductFlowTypesService(StockControlDB dB, ILogger<ProductFlowTypesService> logger)
	{
		_db = dB;
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> SelectAsync(ProductFlowTypeFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.ProductFlowTypes
			.Where(t => t.IsActive);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.Name.Contains(filter.Search));
		}

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<NamedEntityDto>();

		var entities = await query
			.OrderBy(q => q.Name)
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<ProductFlowType>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<NamedEntityDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}
}
