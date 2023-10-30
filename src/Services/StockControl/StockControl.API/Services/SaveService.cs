using Service.Common.Interfaces;

using StockControl.API.DAL.Context;

namespace StockControl.API.Services;

public class SaveService : ISaveService<StockControlDB>
{
	private readonly ILogger<SaveService> _logger;

	public SaveService(ILogger<SaveService> logger)
	{
		_logger = logger;
	}

	public async Task<int> SaveAsync(StockControlDB context, CancellationToken token = default)
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