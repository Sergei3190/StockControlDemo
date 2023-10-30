using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Converters.DB;
using Service.Common.Entities.Base;

namespace StockControl.API.Domain.Stock;

/// <summary>
/// Партия товара
/// </summary>
public class Party : MonitoringEntity
{

	/// <summary>
	/// Номер партии изготовителя
	/// </summary>
	public string Number { get; set; }

	/// <summary>
	/// Уникальный номер партии получателя 
	/// (генерируется на сервере при создании партии, нужно для UI, в случаи если приходят одни и те же партии изготовителя в разных поступлениях)
	/// </summary>
	public string ExtensionNumber { get; set; }

	/// <summary>
	/// Дата изготовления
	/// </summary>
	public DateOnly? CreateDate { get; set; }

	/// <summary>
	/// Время изготовления
	/// </summary>
	public TimeOnly? CreateTime { get; set; }

	public class Map : IEntityTypeConfiguration<Party>
	{
		public void Configure(EntityTypeBuilder<Party> builder)
		{
			builder.ToTable("parties", "stock");

			builder.MapMonitoringEntity();

			builder.Property(x => x.Number).HasColumnName("number").IsRequired();
			builder.Property(x => x.ExtensionNumber).HasColumnName("extension_number").IsRequired();
			builder.Property(x => x.CreateDate).HasColumnName("create_date").HasConversion<DateOnlyConverter>().HasColumnType("date");
			builder.Property(x => x.CreateTime).HasColumnName("create_time").HasConversion<TimeOnlyConverter>().HasColumnType("time(0)");

			builder.HasIndex(x => x.Number);
			builder.HasIndex(x => x.ExtensionNumber).IsUnique();
		}
	}
}