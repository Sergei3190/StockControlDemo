using MediatR;

using Microsoft.Extensions.Caching.Memory;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Select;

public class GetProductFlowTypesHandler : IRequestHandler<GetProductFlowTypesQuery, PaginatedItemsDto<NamedEntityDto>>
{
	private const string CacheKey = "ProductFlowTypes";

	private readonly IProductFlowTypesService _service;
	// используем для статичных справочников/данных
	private readonly IMemoryCache _memoryCache;

	public GetProductFlowTypesHandler(IProductFlowTypesService service, IMemoryCache memoryCache)
	{
		_service = service;
		_memoryCache = memoryCache;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> Handle(GetProductFlowTypesQuery request, CancellationToken cancellationToken)
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