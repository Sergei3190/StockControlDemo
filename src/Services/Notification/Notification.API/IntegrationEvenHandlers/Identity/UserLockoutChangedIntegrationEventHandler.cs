using System.Data.Common;

using EventBus.Interfaces;

using IntegrationEventLogEF;

using Microsoft.EntityFrameworkCore;

using Notification.API.DAL.Context;
using Service.Common.Integration.Events.Identity;

namespace Notification.API.IntegrationEvenHandlers.Identity;

public class UserLockoutChangedIntegrationEventHandler : IIntegrationEventHandler<UserLockoutChangedIntegrationEvent>
{
    private readonly Func<DbConnection, IImportSuccessIntegrationEventLogService> _func;
    private readonly NotificationDB _db;
    private readonly IImportSuccessIntegrationEventLogService _importEventService;
    private readonly ILogger<UserLockoutChangedIntegrationEventHandler> _logger;

    public UserLockoutChangedIntegrationEventHandler(
        Func<DbConnection, IImportSuccessIntegrationEventLogService> func,
        NotificationDB db,
        ILogger<UserLockoutChangedIntegrationEventHandler> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _func = func ?? throw new ArgumentNullException(nameof(func));
        _importEventService = _func(_db.Database.GetDbConnection());
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UserLockoutChangedIntegrationEvent @event)
    {
        // сначала проверяем обрабатывали ли мы полученное событие или может быть к нам пришло событие с неактуальной информацией, те дата его меньше чем дата, 
        // ранее обработанного события
        var handledEvent = await _importEventService.CheckIsExistEventLogAsync(
            new EventLogDto(
                EventId: @event.Id,
                CreationTime: @event.CreationDate,
                EventTypeName: typeof(UserCreatedIntegrationEvent).FullName!)
            );

        // если true, то нам не интересно, откидываем событие
        if (handledEvent)
            return;

        var userInfo = await _db.UsersInfo
            .SingleOrDefaultAsync(u => u.Id == @event.UserId)
            .ConfigureAwait(false);

        if (userInfo is null)
            return;

        userInfo.IsLockout = @event.IsLockout;

        // сохраняем доменный объект и успешный результат обработки интеграционного события, если будет ошибка, то её обработает уже шина сообщений
        await ResilientTransaction.New(_db).ExecuteAsync(async () =>
        {
            await _db.SaveChangesAsync();
            await _importEventService.SaveEventAsync(@event, _db.Database.CurrentTransaction!);
        })
            .ConfigureAwait(false);
    }
}