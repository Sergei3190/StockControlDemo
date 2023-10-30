using Microsoft.EntityFrameworkCore;

using Notification.API.DAL.Context;
using Notification.API.Domain.Notice;
using Notification.API.Infrastructure.Mappers;
using Notification.API.Models.DTO.NotificationType;
using Notification.API.Services.Interfaces;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace Notification.API.Services;

public class NotificationTypesService : INotificationTypesService
{
	private readonly NotificationDB _db;
	private readonly ILogger<NotificationTypesService> _logger;

	public NotificationTypesService(NotificationDB dB, ILogger<NotificationTypesService> logger)
	{
		_db = dB;
		_logger = logger;
	}

	public async Task<PaginatedItemsDto<NamedEntityDto>> SelectAsync(NotificationTypeFilterDto filter)
	{
		ArgumentNullException.ThrowIfNull(filter, nameof(filter));

		var query = _db.NotificationTypes
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

		var dtoItems = entities.Cast<NotificationType>().Select(u => u.CreateDto()!);

		return new PaginatedItemsDto<NamedEntityDto>(filter.Page, filter.PageSize, totalItems, dtoItems);
	}
}
