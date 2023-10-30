using EventBus.Events;

namespace Service.Common.Integration;

/// <summary>
/// Сервис интеграционного события
/// </summary>
public interface IIntegrationEventService
{
    /// <summary>
    /// Добавление и сохранение интеграционного события в бд
    /// </summary>
    Task AddAndSaveEventAsync(IntegrationEvent evt);
}
