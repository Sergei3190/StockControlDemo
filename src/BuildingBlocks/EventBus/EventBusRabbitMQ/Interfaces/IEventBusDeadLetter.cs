using EventBus.Events;
using EventBus.Interfaces;

namespace EventBusRabbitMQ.Interfaces;

/// <summary>
/// Шина недоставленных событий, в неё нельзя публиковать из подписчиком/микросервисов, только отслеживать
/// </summary>
public interface IEventBusDeadLetter
{
    /// <summary>
    /// Заголовки для привязки исходной очереди, которая отправляет сообщения в текущий обменник недоставленных сообщений, 
    /// к обменнику недоставленных сообщений
    /// </summary>
    IDictionary<string, object> Headers { get; set; }

    /// <summary>
    /// Запустить обменник недоставленных сообщений
    /// </summary>
    void StartDeadLetter(CancellationToken cancel);

    /// <summary>
    /// Подписаться на событие
    /// </summary>
    /// <typeparam name="T"> Событие интеграции, на которое нужно подписаться </typeparam>
    /// <typeparam name="TH"> Обработчик события интеграции(или метод обратного вызова) с именем, IIntegrationEventHandler<T>, 
    /// который будет выполняться, когда микрослужба-получатель получит это сообщение о событии интеграции.</typeparam>
    void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>;

    /// <summary>
    /// Отписаться от события
    /// </summary>
    /// <typeparam name="T"> Событие интеграции, на которое нужно подписаться </typeparam>
    /// <typeparam name="TH"> Обработчик события интеграции(или метод обратного вызова) с именем, IIntegrationEventHandler<T>, 
    /// который будет выполняться, когда микрослужба-получатель получит это сообщение о событии интеграции.</typeparam>
    void Unsubscribe<T, TH>()
        where TH : IIntegrationEventHandler<T>
        where T : IntegrationEvent;
}