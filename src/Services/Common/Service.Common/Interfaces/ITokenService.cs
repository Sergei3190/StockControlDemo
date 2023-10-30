namespace Service.Common.Interfaces;

/// <summary>
/// Интерфейс получения токена авторизации
/// </summary>
public interface ITokenService
{
	Task<string> GetTokenAsync(CancellationToken cancellationToken = default);
}
