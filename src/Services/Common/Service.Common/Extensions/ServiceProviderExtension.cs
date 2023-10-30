using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Service.Common.DTO;
using Service.Common.Interfaces;

namespace Service.Common.Extensions;

public static class ServiceProviderExtension
{
    public static async Task<IServiceProvider> InitialDbAsync(this IServiceProvider services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        await using (var scope = services.CreateAsyncScope())
        {
            var initService = scope.ServiceProvider.GetRequiredService<IDbInitializerService>();

            DbInitializerDto dto = null!;

            try
            {
                dto = configuration.GetRequiredSection("DB").Get<DbInitializerDto>()!;
            }
            catch (InvalidOperationException ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbInitializerDto>>();
                logger.LogError("Не найдена секция инициализации БД {ex}", ex);
                throw;
            }

            await initService.InitializeAsync(dto).ConfigureAwait(false);
        }

        return services;
    }
}
