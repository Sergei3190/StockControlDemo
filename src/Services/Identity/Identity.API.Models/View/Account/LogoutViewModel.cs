using Identity.API.Models.Input.Account;

namespace Identity.API.Models.View.Account;

public class LogoutViewModel : LogoutInputModel
{
    public bool ShowLogoutPrompt { get; set; } = true;
}
