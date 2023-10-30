namespace EventBus.Interfaces;

/// <summary>
/// Обработчик динамических событий шины (без привязки к классу)
/// </summary>
public interface IDynamicIntegrationEventHandler
{
	Task Handle(dynamic eventData);
}
