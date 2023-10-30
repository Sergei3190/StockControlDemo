using System.Data.Common;

using EventBus.Interfaces;

using IntegrationEventLogEF;

using Microsoft.EntityFrameworkCore;

using Notification.API.DAL.Context;
using Service.Common.Integration.Events.Identity;
using Service.Common.Integration.Mappers;

namespace Notification.API.IntegrationEvenHandlers.Identity;

public class UserCreatedIntegrationEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    private readonly Func<DbConnection, IImportSuccessIntegrationEventLogService> _func;
    private readonly NotificationDB _db;
    private readonly IImportSuccessIntegrationEventLogService _importEventService;
    private readonly ILogger<UserCreatedIntegrationEventHandler> _logger;

    public UserCreatedIntegrationEventHandler(
        Func<DbConnection, IImportSuccessIntegrationEventLogService> func,
        NotificationDB db,
        ILogger<UserCreatedIntegrationEventHandler> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _func = func ?? throw new ArgumentNullException(nameof(func));
        _importEventService = _func(_db.Database.GetDbConnection());
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UserCreatedIntegrationEvent @event)
    {
        // сначала проверяем обрабатывали ли мы полученное событие или может быть к нам пришло событие с неактуальной информацией, те дата его меньше чем дата, 
        // ранее обработанного события
        var handledEvent = await _importEventService.CheckIsExistEventLogAsync(
            new EventLogDto(
                EventId: @event.Id,
                CreationTime: @event.CreationDate,
                EventTypeName: typeof(UserCreatedIntegrationEvent).FullName!)
            )
            .ConfigureAwait(false);

        // если true, то нам не интересно, откидываем событие
        if (handledEvent)
            return;

        // делаем дополнительную проверку на наличие пользователя в системе
        var isUserExists = await _db.UsersInfo
            .AnyAsync(u => u.Id == @event.UserId || u.Email == @event.Email || u.Name == @event.Name)
            .ConfigureAwait(false);

        // если уже есть такой пользователь, то откидываем событие
        if (isUserExists)
            return;

        var userInfo = @event.ToUserInfo();

        await _db.UsersInfo.AddAsync(userInfo!).ConfigureAwait(false);

        // сохраняем доменный объект и успешный результат обработки интеграционного события, если будет ошибка, то её обработает уже шина сообщений
        await ResilientTransaction.New(_db).ExecuteAsync(async () =>
        {
            await _db.SaveChangesAsync();
            await _importEventService.SaveEventAsync(@event, _db.Database.CurrentTransaction!);
        })
            .ConfigureAwait(false);
    }
}