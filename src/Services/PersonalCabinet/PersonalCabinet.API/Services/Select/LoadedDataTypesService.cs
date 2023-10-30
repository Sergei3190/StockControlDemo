using Microsoft.EntityFrameworkCore;

using PersonalCabinet.API.DAL.Context;
using PersonalCabinet.API.Domain.Person;
using PersonalCabinet.API.Infrastructure.Mappers;
using PersonalCabinet.API.Models.DTO.LoadedDataType;
using PersonalCabinet.API.Services.Interfaces.Select;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.Services.Select;

public class LoadedDataTypesService : ILoadedDataTypesService
{
	private readonly PersonalCabinetDB _db;
	private readonly ILogger<LoadedDataTypesService> _logger;

	public LoadedDataTypesService(PersonalCabinetDB dB, ILogger<LoadedDataTypesService> logger)
	{
		_db = dB;
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> SelectAsync(LoadedDataTypeFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.LoadedDataTypes
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

		var dtoItems = entities.Cast<LoadedDataType>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<NamedEntityDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}
}
