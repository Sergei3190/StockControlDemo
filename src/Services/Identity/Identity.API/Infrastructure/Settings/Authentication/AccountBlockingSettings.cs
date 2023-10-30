namespace Identity.API.Infrastructure.Settings.Authentication;

public class AccountBlockingSettings
{
    public bool? AllowedForNewUsers { get; set; }
    public int MaxLoginAttempts { get; set; }
    public int? LockoutMinutes { get; set; }
}
