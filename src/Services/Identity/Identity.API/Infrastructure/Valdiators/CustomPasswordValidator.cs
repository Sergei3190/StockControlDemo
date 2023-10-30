using Microsoft.AspNetCore.Identity;

namespace Identity.API.Infrastructure.Valdiators;

/// <summary>
/// Пользовательская проверка пароля
/// </summary>
/// <typeparam name="TUser"></typeparam>
public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : IdentityUser<Guid>
{
    public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
    {
        var username = await manager.GetUserNameAsync(user);

        if (username.ToLower().Equals(password.ToLower()))
            return IdentityResult.Failed(new IdentityError { Description = "Имя пользователя и пароль не могут быть одинаковыми.", Code = "SameUserPass" });

        if (password.ToLower().Contains("password"))
            return IdentityResult.Failed(new IdentityError { Description = "Слово password не допускается для пароля.", Code = "PasswordContainsPassword" });

        if (await manager.CheckPasswordAsync(user, password))
            return IdentityResult.Failed(new IdentityError { Description = "Новый пароль не может быть таким же как и старый пароль.", Code = "PasswordRepeat" });

        return IdentityResult.Success;
    }
}
