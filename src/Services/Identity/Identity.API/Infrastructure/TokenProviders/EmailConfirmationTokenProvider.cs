using Identity.API.Infrastructure.Options.TokenProvider;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.API.Infrastructure.TokenProviders;

/// <summary>
/// Клиентксий токен провайдер для подтверждения электронной почты, необходим чтобы установить свой период жизни токена
/// </summary>
public class EmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : IdentityUser<Guid>
{
    public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider,
        IOptions<EmailConfirmationTokenProviderOptions> options,
        ILogger<DataProtectorTokenProvider<TUser>> logger)
        : base(dataProtectionProvider, options, logger)
    {
    }
}
