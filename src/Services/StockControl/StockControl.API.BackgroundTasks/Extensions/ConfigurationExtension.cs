using Microsoft.EntityFrameworkCore;

namespace StockControl.API.BackgroundTask.Extensions;

public static class ConfigurationExtension
{
	public static string GetDbConnectionString(this IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

		var dbType = configuration["Db:Type"];
		var connectionString = configuration.GetConnectionString(dbType!);

		if (connectionString is null)
			throw new NullReferenceException($"Отсутствует строка подключения к БД {nameof(connectionString)}");

		return connectionString;
	}
}
