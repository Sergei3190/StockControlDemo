using Service.Common.DTO;

namespace Service.Common.Interfaces;

/// <summary>
/// Интерфейс инициализации бд микросервиса
/// </summary>
public interface IDbInitializerService
{
    Task InitializeAsync(DbInitializerDto dto, CancellationToken cancel = default);
}