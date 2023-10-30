using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PersonalCabinet.API.Domain.Person.Abstractions;

namespace PersonalCabinet.API.Domain.Person;

/// <summary>
/// Фото персоны
/// </summary>
public class PersonPhoto : LoadedData
{
	public class Map : IEntityTypeConfiguration<PersonPhoto>
	{
		public void Configure(EntityTypeBuilder<PersonPhoto> builder)
		{
			builder.ToTable("photos", "person");

			builder.MapLoadedDataEntity();

			builder.HasOne(x => x.LoadedDataType).WithMany(l => l.Photos).HasForeignKey(x => x.LoadedDataTypeId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}