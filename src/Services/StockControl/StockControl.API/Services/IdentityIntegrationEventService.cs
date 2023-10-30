using System.Data.Common;

using EventBus.Events;

using IntegrationEventLogEF.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

using Service.Common.Integration;

using StockControl.API.DAL.Context;

namespace StockControl.API.Services;

public class IdentityIntegrationEventService : IIntegrationEventService
{
	private readonly Func<DbConnection, IExportIntegrationEventLogService> _func;
	private readonly StockControlDB _db;
	private readonly IExportIntegrationEventLogService _eventLogService;
	private readonly ILogger<IdentityIntegrationEventService> _logger;

	public IdentityIntegrationEventService(
		StockControlDB db,
		Func<DbConnection, IExportIntegrationEventLogService> func,
		ILogger<IdentityIntegrationEventService> logger)
	{
		_db = db ?? throw new ArgumentNullException(nameof(db));
		_func = func ?? throw new ArgumentNullException(nameof(func));
		_eventLogService = _func(_db.Database.GetDbConnection());
		_logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task AddAndSaveEventAsync(IntegrationEvent evt)
	{
		_logger.LogInformation("Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

		await _eventLogService.SaveEventAsync(evt, _db.GetCurrentTransaction());
	}
}
