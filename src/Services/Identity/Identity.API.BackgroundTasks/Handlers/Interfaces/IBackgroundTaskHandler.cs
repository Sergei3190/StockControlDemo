namespace Identity.API.BackgroundTasks.Handlers.Interfaces;

/// <summary>
/// Базовый сервис обработки фоновых задач
/// </summary>
public interface IBackgroundTaskHandler
{
    Task HandleAsync(IServiceProvider provider, CancellationToken cancel);
}
