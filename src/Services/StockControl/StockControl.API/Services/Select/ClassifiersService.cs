using Microsoft.EntityFrameworkCore;

using Service.Common.DTO;

using StockControl.API.DAL.Context;
using StockControl.API.Domain.Stock;
using StockControl.API.Infrastructure.Mappers;
using StockControl.API.Models.DTO.Classifier;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.Services.Select;

public class ClassifiersService : IClassifiersService
{
	private readonly StockControlDB _db;
	private readonly ILogger<ClassifiersService> _logger;

	public ClassifiersService(StockControlDB dB, ILogger<ClassifiersService> logger)
	{
		_db = dB;
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<ClassifierDto>> SelectAsync(ClassifierFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.Classifiers
			.Where(t => t.IsActive);

		if (!string.IsNullOrEmpty(filter.Search))
		{
			query = query.Where(q => q.Name.Contains(filter.Search));
		}

		var totalItems = await query.CountAsync()
			.ConfigureAwait(false);

		if (totalItems == 0)
			return new PaginatedItemsDto<ClassifierDto>();

		var entities = await query
			.OrderBy(q => q.Name)
			.Skip(filter.Skip)
			.Take(filter.Take)
			.AsNoTracking()
			.ToArrayAsync()
			.ConfigureAwait(false);

		var dtoItems = entities.Cast<Classifier>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<ClassifierDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}
}
