using Notification.API.DAL.Context;

using Service.Common.Interfaces;

namespace Notification.API.Services;
public class SaveService : ISaveService<NotificationDB>
{
	private readonly ILogger<SaveService> _logger;

	public SaveService(ILogger<SaveService> logger)
	{
		_logger = logger;
	}

	public async Task<int> SaveAsync(NotificationDB context, CancellationToken token = default)
	{
		try
		{
			return await context.SaveChangesAsync(token).ConfigureAwait(false);
		}
		catch (Exception ex)
		{
			_logger.LogError("Ошибка при сохранении данных в БД : {error}", ex.Message);
			throw;
		}
	}
}