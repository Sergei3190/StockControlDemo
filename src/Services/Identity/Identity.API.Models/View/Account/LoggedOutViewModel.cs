namespace Identity.API.Models.View.Account;

public class LoggedOutViewModel
{
    /// <summary>
    /// Url для возврата в приложение
    /// </summary>
    public string PostLogoutRedirectUri { get; set; }
    public string ClientName { get; set; }

    /// <summary>
    /// Url внешнего провайдера
    /// </summary>
    public string SignOutIframeUrl { get; set; }

    /// <summary>
    /// Автоматическое перенаправление после выхода из системы
    /// </summary>
    public bool AutomaticRedirectAfterSignOut { get; set; }

    public string LogoutId { get; set; }

    /// <summary>
    /// Тритегр внешней утентификации
    /// </summary>
    public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
    
    /// <summary>
    /// Схема аутентификации внешнего провайдера
    /// </summary>
    public string ExternalAuthenticationScheme { get; set; }
}
