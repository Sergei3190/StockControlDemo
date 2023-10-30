using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StockControl.API.Domain.Stock.Abstractions;

namespace StockControl.API.Domain.Stock;

/// <summary>
/// Справочник номенклатуры
/// </summary>
public class Nomenclature : ClassifierItem
{
    public class Map : IEntityTypeConfiguration<Nomenclature>
    {
        public void Configure(EntityTypeBuilder<Nomenclature> builder)
        {
            builder.ToTable("nomenclatures", "stock");

            builder.MapClassifierItemEntity();

			builder.HasOne(x => x.Classifier).WithMany(i => i.Nomenclatures).HasForeignKey(i => i.ClassifierId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}