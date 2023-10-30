using System.Data.Common;

using EventBus.Events;

using Identity.API.DAL.Context;

using IntegrationEventLogEF.Services.Interfaces;

using Microsoft.EntityFrameworkCore;

using Service.Common.Integration;

namespace Identity.API.Services;

public class IdentityIntegrationEventService : IIntegrationEventService
{
    private readonly Func<DbConnection, IExportIntegrationEventLogService> _func;
    private readonly IdentityDB _db;
    private readonly IExportIntegrationEventLogService _eventLogService;
    private readonly ILogger<IdentityIntegrationEventService> _logger;

    public IdentityIntegrationEventService(
        IdentityDB db,
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
