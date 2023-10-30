﻿using IntegrationEventLogEF;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

using PersonalCabinet.API.DAL.Context;

namespace PersonalCabinet.API.Infrastructure.Extensions;

public static class DbContextExtension
{
    public static IServiceCollection AddPersonalCabinetDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        static void ConfigureSqlOptions(SqlServerDbContextOptionsBuilder sqlOptions)
        {
            const string assemblerName = "PersonalCabinet.API.DAL";

            sqlOptions.MigrationsAssembly(assemblerName);

            // Отказоустойчивость подключения: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        };

        var dbType = configuration["Db:Type"];
        var connectionString = configuration.GetConnectionString(dbType!);

        switch (dbType)
        {
            case "DockerDb":
            case "SqlServer":
                services.AddDbContext<PersonalCabinetDB>(opt =>
                {
                    opt.UseSqlServer(connectionString, ConfigureSqlOptions);
                });
                services.AddDbContext<IntegrationEventLogContext>(opt =>
                {
                    opt.UseSqlServer(connectionString, ConfigureSqlOptions);
                });
                break;
        }

        return services;
    }
}
