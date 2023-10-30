using MediatR;

using Microsoft.Extensions.Caching.Memory;

using Service.Common.DTO;

using StockControl.API.MediatR.Queries.Select;
using StockControl.API.Models.DTO.Classifier;
using StockControl.API.Services.Interfaces.Select;

namespace StockControl.API.MediatR.Handlers.QueryHandlers.Select;

public class GetClassifiersHandler : IRequestHandler<GetClassifiersQuery, PaginatedItemsDto<ClassifierDto>>
{
	private const string CacheKey = "Classifiers";

	private readonly IClassifiersService _service;
	// используем для статичных справочников/данных
	private readonly IMemoryCache _memoryCache;

	public GetClassifiersHandler(IClassifiersService service, IMemoryCache memoryCache)
	{
		_service = service;
		_memoryCache = memoryCache;
	}

	public async Task<PaginatedItemsDto<ClassifierDto>> Handle(GetClassifiersQuery request, CancellationToken cancellationToken)
	{
		PaginatedItemsDto<ClassifierDto> types;

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