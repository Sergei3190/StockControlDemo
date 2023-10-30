namespace Identity.API.Infrastructure.Settings.Authentication;

public class AuthenticationSettings
{
    public PasswordSettings Password { get; set; }
    public UserSettings User { get; set; }
    public SignInSettings SignIn { get; set; }
    public AccountBlockingSettings AccountBlocking { get; set; }
    public int CookieLifetime { get; set; }
    public int TokenLifespan { get; set; }
    public int EmailTokenLifespan { get; set; }
}
