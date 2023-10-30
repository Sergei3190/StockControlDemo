using Identity.API.Models.Input.Account;
using Identity.API.Models.View.Account.Interfaces;

namespace Identity.API.Models.View.Account;

public class RegisterViewModel : RegisterInputModel, IAccountSetting
{
    public bool AllowRememberLogin { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;
}
