using Email.Service;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Service.Common.Configs;

namespace Service.Common.Extensions;

public static class EmailExtension
{
    public static IServiceCollection AddDefaultEmailService(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        var section = Config.GetConfiguration().GetRequiredSection("Email");

        var emailConfig = section.Get<EmailConfiguration>()!;

        services
            .Configure<EmailConfiguration>(section)
            .AddSingleton(emailConfig)
            .AddScoped<IEmailSender, EmailSender>();

        return services;
    }
}
