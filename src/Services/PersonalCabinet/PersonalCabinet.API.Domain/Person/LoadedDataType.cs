using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace PersonalCabinet.API.Domain.Person;

/// <summary>
/// Типы загруженной информации о пользователе
/// </summary>
public class LoadedDataType : DictionaryEntity
{
	public static LoadedDataType[] LoadedDataTypes = new[]
	{
		new LoadedDataType()
		{
			Id = Guid.Parse("79DFBB7D-C16F-46B4-A55C-07D8B2A09D0A"),
			Name = "Фотография",
			Mnemo = "Photo",
			IsActive = true,
		},
		new LoadedDataType()
		{
			Id = Guid.Parse("0B50547C-8E76-4FA9-82C7-F8280093F890"),
			Name = "Документ",
			Mnemo = "Document",
			IsActive = true,
		}
	};

	public LoadedDataType()
	{
		Photos = new HashSet<PersonPhoto>();
	}

	public ICollection<PersonPhoto> Photos { get; set; }
	public ICollection<PersonDocument> Documents { get; set; }

	public class Map : IEntityTypeConfiguration<LoadedDataType>
	{
		public void Configure(EntityTypeBuilder<LoadedDataType> builder)
		{
			builder.ToTable("loaded_data_types", "person");

			builder.HasMany(x => x.Photos).WithOne(p => p.LoadedDataType).HasForeignKey(p => p.LoadedDataTypeId);
			builder.HasMany(x => x.Documents).WithOne(p => p.LoadedDataType).HasForeignKey(p => p.LoadedDataTypeId);

			builder.MapDictionaryEntity();
		}
	}
}