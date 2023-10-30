using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StockControl.API.Domain.Stock.Abstractions;

namespace StockControl.API.Domain.Stock;

/// <summary>
/// Наличие товара на складах
/// </summary>
public class StockAvailability : Product
{
	/// <summary>
	/// Поступление
	/// </summary>
	public Guid? ReceiptId { get; set; }
	public Receipt? Receipt { get; set; }

	/// <summary>
	/// Перемещение
	/// </summary>
	public Guid? MovingId { get; set; }
	public Moving? Moving { get; set; }

	/// <summary>
	/// Списание
	/// </summary>
	public Guid? WriteOffId { get; set; }
	public WriteOff? WriteOff { get; set; }

	public class Map : IEntityTypeConfiguration<StockAvailability>
	{
		public void Configure(EntityTypeBuilder<StockAvailability> builder)
		{
			builder.ToTable("stock_availabilities", "stock");

			builder.MapProductEntity();

			builder.Property(x => x.ReceiptId).HasColumnName("receipt_id");
			builder.Property(x => x.MovingId).HasColumnName("moving_id");
			builder.Property(x => x.WriteOffId).HasColumnName("write_off_id");

			builder.HasOne(x => x.Party).WithMany().HasForeignKey(i => i.PartyId).OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.Receipt).WithOne().HasForeignKey<StockAvailability>(u => u.ReceiptId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.Moving).WithOne().HasForeignKey<StockAvailability>(u => u.MovingId).OnDelete(DeleteBehavior.NoAction);
			builder.HasOne(x => x.WriteOff).WithOne().HasForeignKey<StockAvailability>(u => u.WriteOffId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}