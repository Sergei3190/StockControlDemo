using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace StockControl.API.Domain.Stock;

/// <summary>
/// Типы движения товара
/// </summary>
public class ProductFlowType : DictionaryEntity
{
	public static ProductFlowType[] ProductFlowTypes = new[]
	{
		new ProductFlowType()
		{
			Id = Guid.Parse("33960D0E-7EC1-4921-B5D0-9E1D3C60C2AC"),
			Name = "Поступление",
			Mnemo = "Receipt",
			IsActive = true,
		},
		new ProductFlowType()
		{
			Id = Guid.Parse("7428F404-4D43-4BC7-BB3E-A9590E9F65FD"),
			Name = "Перемещение",
			Mnemo = "Moving",
			IsActive = true,
		},
		new ProductFlowType()
		{
			Id = Guid.Parse("DD495421-0565-4B1C-A0C8-52DDA23DCE86"),
			Name = "Списание",
			Mnemo = "WriteOff",
			IsActive = true,
		}
	};

	public ProductFlowType()
	{
		WriteOffs = new HashSet<WriteOff>();
		Movings = new HashSet<Moving>();
		Receipts = new HashSet<Receipt>();
	}

	public ICollection<WriteOff> WriteOffs { get; set; }
	public ICollection<Moving> Movings { get; set; }
	public ICollection<Receipt> Receipts { get; set; }

	public class Map : IEntityTypeConfiguration<ProductFlowType>
	{
		public void Configure(EntityTypeBuilder<ProductFlowType> builder)
		{
			builder.ToTable("product_flow_types", "stock");

			builder.HasMany(x => x.WriteOffs).WithOne(p => p.ProductFlowType).HasForeignKey(p => p.ProductFlowTypeId);
			builder.HasMany(x => x.Movings).WithOne(p => p.ProductFlowType).HasForeignKey(p => p.ProductFlowTypeId);
			builder.HasMany(x => x.Receipts).WithOne(p => p.ProductFlowType).HasForeignKey(p => p.ProductFlowTypeId);

			builder.MapDictionaryEntity();
		}
	}
}