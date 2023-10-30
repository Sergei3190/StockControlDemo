namespace Identity.API.Models.View.Account;

public class LoggedOutViewModel
{
    /// <summary>
    /// Url ��� �������� � ����������
    /// </summary>
    public string PostLogoutRedirectUri { get; set; }
    public string ClientName { get; set; }

    /// <summary>
    /// Url �������� ����������
    /// </summary>
    public string SignOutIframeUrl { get; set; }

    /// <summary>
    /// �������������� ��������������� ����� ������ �� �������
    /// </summary>
    public bool AutomaticRedirectAfterSignOut { get; set; }

    public string LogoutId { get; set; }

    /// <summary>
    /// ������� ������� �������������
    /// </summary>
    public bool TriggerExternalSignout => ExternalAuthenticationScheme != null;
    
    /// <summary>
    /// ����� �������������� �������� ����������
    /// </summary>
    public string ExternalAuthenticationScheme { get; set; }
}
