using EventBus.Events;

namespace EventBus.Interfaces;

/// <summary>
/// Менеджер подписок на события шины
/// </summary>
public interface IEventBusSubscriptionsManager
{
    bool IsEmpty { get; }

    event EventHandler<string> OnEventRemoved;

    /// <summary>
    /// Добавить динамический обработчик на событие
    /// </summary>
    void AddDynamicSubscription<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

    /// <summary>
    /// Добавить обработчик на событие
    /// </summary>
    void AddSubscription<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// Удалить обработчик на событие
    /// </summary>
    void RemoveSubscription<T, TH>()
         where TH : IIntegrationEventHandler<T>
         where T : IntegrationEvent;

    /// <summary>
    /// Удалить динамический обработчик на событие
    /// </summary>
    void RemoveDynamicSubscription<TH>(string eventName)
        where TH : IDynamicIntegrationEventHandler;

    /// <summary>
    /// Имеются ли подписчики на событие
    /// </summary>
    bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent;

    /// <summary>
    /// Имеются ли подписчики на событие
    /// </summary>
    bool HasSubscriptionsForEvent(string eventName);

    /// <summary>
    /// Получить тип сообщения по его имени
    /// </summary>
    Type GetEventTypeByName(string eventName);

    /// <summary>
    /// Очистить список обработчиков
    /// </summary>
    void Clear();

    /// <summary>
    /// Получить обработчики на событие
    /// </summary>
    IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent;

    /// <summary>
    /// Получить обработчики на событие
    /// </summary>
    IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName);

    /// <summary>
    /// Получить название события
    /// </summary>
    string GetEventKey<T>();
}