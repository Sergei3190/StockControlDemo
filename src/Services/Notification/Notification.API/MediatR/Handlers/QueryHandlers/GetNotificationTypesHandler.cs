using MediatR;

using Microsoft.Extensions.Caching.Memory;

using Notification.API.MediatR.Queries;
using Notification.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace Notification.API.MediatR.Handlers.QueryHandlers;

public class GetNotificationTypesHandler : IRequestHandler<GetNotificationTypesQuery, PaginatedItemsDto<NamedEntityDto>>
{
	private const string CacheKey = "NotificationTypes";

	private readonly INotificationTypesService _service;
	// используем для статичных справочников/данных
	private readonly IMemoryCache _memoryCache;

	public GetNotificationTypesHandler(INotificationTypesService service, IMemoryCache memoryCache)
	{
		_service = service;
		_memoryCache = memoryCache;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> Handle(GetNotificationTypesQuery request, CancellationToken cancellationToken)
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