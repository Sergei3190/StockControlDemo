namespace Identity.API.Models.View.Account.Interfaces;

public interface IAccountSetting
{
    public bool AllowRememberLogin { get; set; }
    public bool EnableLocalLogin { get; set; }
}