using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using PersonalCabinet.API.Domain.Person.Abstractions;

namespace PersonalCabinet.API.Domain.Person;

/// <summary>
/// Документ персоны
/// </summary>
public class PersonDocument : LoadedData
{
	public class Map : IEntityTypeConfiguration<PersonDocument>
	{
		public void Configure(EntityTypeBuilder<PersonDocument> builder)
		{
			builder.ToTable("documents", "person");

			builder.MapLoadedDataEntity();

			builder.HasOne(x => x.LoadedDataType).WithMany(l => l.Documents).HasForeignKey(x => x.LoadedDataTypeId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}