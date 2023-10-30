using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StockControl.API.Domain.Stock.Abstractions;

namespace StockControl.API.Domain.Stock;

/// <summary>
/// Справочник организаций
/// </summary>
public class Organization : ClassifierItem
{
    public class Map : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("organizations", "stock");

            builder.MapClassifierItemEntity();

			builder.HasOne(x => x.Classifier).WithMany(i => i.Organizations).HasForeignKey(i => i.ClassifierId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}