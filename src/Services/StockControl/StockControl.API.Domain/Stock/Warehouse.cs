using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StockControl.API.Domain.Stock.Abstractions;

namespace StockControl.API.Domain.Stock;

/// <summary>
/// Справочник складов
/// </summary>
public class Warehouse : ClassifierItem
{
    public class Map : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("warehouses", "stock");

            builder.MapClassifierItemEntity();

			builder.HasOne(x => x.Classifier).WithMany(i => i.Warehouses).HasForeignKey(i => i.ClassifierId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}