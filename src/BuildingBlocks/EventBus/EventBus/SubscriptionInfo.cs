namespace EventBus;

/// <summary>
/// Описание обработчика
/// </summary>
public class SubscriptionInfo
{
    /// <summary>
    /// Динамическое ли сообщение (не типизированное)
    /// </summary>
    public bool IsDynamic { get; }

    /// <summary>
    /// Тип обработчика
    /// </summary>
    public Type HandlerType { get; }

    /// <summary>
    /// Конструктор
    /// </summary>
    private SubscriptionInfo(bool isDynamic, Type handlerType)
    {
        IsDynamic = isDynamic;
        HandlerType = handlerType;
    }

    /// <summary>
    /// Создать обработчик для динамического сообщения
    /// </summary>
    public static SubscriptionInfo Dynamic(Type handlerType)
    {
        return new SubscriptionInfo(true, handlerType);
    }

    /// <summary>
    /// Создать обработчик типизированного сообщения
    /// </summary>
    public static SubscriptionInfo Typed(Type handlerType)
    {
        return new SubscriptionInfo(false, handlerType);
    }
}
