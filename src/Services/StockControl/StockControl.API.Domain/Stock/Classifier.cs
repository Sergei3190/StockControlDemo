using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace StockControl.API.Domain.Stock;

/// <summary>
/// Статический классификатор справочников приложения
/// </summary>
public class Classifier : DictionaryEntity
{
	public static Classifier[] Classifiers = new[]
	{
		new Classifier()
		{
			Id = Guid.Parse("DD049317-5FC5-44E8-87DB-FE72AD68C14E"),
			Name = "Организации",
			Mnemo = "Organizations",
			IsActive = true,
		},
		new Classifier()
		{
			Id = Guid.Parse("355EF3D2-1C21-45B2-A000-EC69BB170FB6"),
			Name = "Склады",
			Mnemo = "Warehouses",
			IsActive = true,
		},
		new Classifier()
		{
			Id = Guid.Parse("BDF6CCB8-A4DD-48E3-B255-FF5C8E8B3EA2"),
			Name = "Номенклатура",
			Mnemo = "Nomenclature",
			IsActive = true,
		}
	};

	public Classifier()
	{
		Organizations = new HashSet<Organization>();
		Nomenclatures = new HashSet<Nomenclature>();
		Warehouses = new HashSet<Warehouse>();
	}

	public ICollection<Organization> Organizations { get; set; }
	public ICollection<Nomenclature> Nomenclatures { get; set; }
	public ICollection<Warehouse> Warehouses { get; set; }

	public class Map : IEntityTypeConfiguration<Classifier>
	{
		public void Configure(EntityTypeBuilder<Classifier> builder)
		{
			builder.ToTable("classifiers", "stock");

			builder.MapDictionaryEntity();

			builder.HasMany(i => i.Organizations).WithOne(x => x.Classifier).HasForeignKey(x => x.ClassifierId);
			builder.HasMany(i => i.Nomenclatures).WithOne(x => x.Classifier).HasForeignKey(x => x.ClassifierId);
			builder.HasMany(i => i.Warehouses).WithOne(x => x.Classifier).HasForeignKey(x => x.ClassifierId);
		}
	}
}