using EventBus.Events;

namespace EventBus.Interfaces;

/// <summary>
/// Основной интерфейс шины событий 
/// </summary>
public interface IEventBus
{
	/// <summary>
	/// Опубликовать сообщение
	/// </summary>
	/// <param name="event"></param>
	void Publish(IntegrationEvent @event);

	/// <summary>
	/// Подписаться на событие
	/// </summary>
	/// <typeparam name="T"> Событие интеграции, на которое нужно подписаться </typeparam>
	/// <typeparam name="TH"> Обработчик события интеграции(или метод обратного вызова) с именем, IIntegrationEventHandler<T>, 
	/// который будет выполняться, когда микрослужба-получатель получит это сообщение о событии интеграции.</typeparam>
	void Subscribe<T, TH>()
		where T : IntegrationEvent
		where TH : IIntegrationEventHandler<T>;

	void SubscribeDynamic<TH>(string eventName)
	   where TH : IDynamicIntegrationEventHandler;

	/// <summary>
	/// Отписаться от события
	/// </summary>
	/// <typeparam name="T"> Событие интеграции, на которое нужно подписаться </typeparam>
	/// <typeparam name="TH"> Обработчик события интеграции(или метод обратного вызова) с именем, IIntegrationEventHandler<T>, 
	/// который будет выполняться, когда микрослужба-получатель получит это сообщение о событии интеграции.</typeparam>
	void Unsubscribe<T, TH>()
		where TH : IIntegrationEventHandler<T>
		where T : IntegrationEvent;

	void UnsubscribeDynamic<TH>(string eventName)
		where TH : IDynamicIntegrationEventHandler;
}
