using IntegrationEventLogEF.Entities;

using Microsoft.EntityFrameworkCore;

namespace IntegrationEventLogEF;

// В текущем приложении все унаследованные от DbContext классы названы с окончанием DB, те указывая, что создаётся отдельная бд.
// Текущий класс назван как IntegrationEventLogContext, подчеркивая что мы не создаём отдельную бд
// для размещения таблиц данного контекста, а используем БД исходного микросервиса.

public class IntegrationEventLogContext : DbContext
{
	/// <summary>
	/// Исходящие интеграционные события
	/// </summary>
	public DbSet<ExportIntegrationEventLog> ExportIntegrationEventLogs { get; set; }

	/// <summary>
	/// Входящие успешно обработанные интеграционные события
	/// </summary>
	public DbSet<ImportSuccessIntegrationEventLog> ImportSuccessIntegrationEventLogs { get; set; }

	public IntegrationEventLogContext(DbContextOptions<IntegrationEventLogContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		Map(builder);
	}

	private static void Map(ModelBuilder builder)
	{
		builder.ApplyConfiguration(new ExportIntegrationEventLog.Map());
		builder.ApplyConfiguration(new ImportSuccessIntegrationEventLog.Map());
	}
}
