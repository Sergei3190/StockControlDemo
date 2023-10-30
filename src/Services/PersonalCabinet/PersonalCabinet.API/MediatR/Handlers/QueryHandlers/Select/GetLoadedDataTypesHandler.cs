using MediatR;

using Microsoft.Extensions.Caching.Memory;

using PersonalCabinet.API.MediatR.Queries.Select;
using PersonalCabinet.API.Services.Interfaces.Select;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.MediatR.Handlers.QueryHandlers.Select;

public class GetLoadedDataTypesHandler : IRequestHandler<GetLoadedDataTypesQuery, PaginatedItemsDto<NamedEntityDto>>
{
	private const string CacheKey = "LoadedDataTypes";

	private readonly ILoadedDataTypesService _service;
	// используем для статичных справочников/данных
	private readonly IMemoryCache _memoryCache;

	public GetLoadedDataTypesHandler(ILoadedDataTypesService service, IMemoryCache memoryCache)
	{
		_service = service;
		_memoryCache = memoryCache;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> Handle(GetLoadedDataTypesQuery request, CancellationToken cancellationToken)
	{
		PaginatedItemsDto<NamedEntityDto> types;

		var key = $"{CacheKey} page: {request.Filter.Page} pageSize: {request.Filter.PageSize} search: {request.Filter.Search}";

		var result = _memoryCache.TryGetValue(key, out types!);

		if (!result)
		{
			types = await _service.SelectAsync(request.Filter).ConfigureAwait(false);

			_memoryCache.Set(key, types);
		}

		return types;
	}
}