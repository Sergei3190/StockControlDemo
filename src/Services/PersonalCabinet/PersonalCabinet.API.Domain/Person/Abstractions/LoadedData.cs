using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace PersonalCabinet.API.Domain.Person.Abstractions;

/// <summary>
/// Загруженные данные о пользователе
/// </summary>
public abstract class LoadedData : MonitoringEntity
{
    public Guid CardId { get; set; }
    public Card Card { get; set; }

    /// <summary>
    /// Идентификатор фото в файловом хранилище, например в монго
    /// </summary>
    public Guid ExternalId { get; set; }

    /// <summary>
    /// Имя файла, необходимо для поиска в интерфейсе пользователя
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Тип загруженных данных
    /// </summary>
    public Guid LoadedDataTypeId { get; set; }
    public LoadedDataType LoadedDataType { get; set; }

    public override string ToString() => $"({nameof(Id)}: {Id}):" +
        $"{nameof(CardId)}: {CardId}" +
        $"{nameof(ExternalId)}: {ExternalId}" +
        $"{nameof(FileName)}: {FileName}" +
        $"{nameof(LoadedDataTypeId)}: {LoadedDataTypeId}";
}

public static class LoadedDataConfiguraton
{
	public static void MapLoadedDataEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : LoadedData
	{
		builder.MapMonitoringEntity();

		builder.Property(x => x.CardId).HasColumnName("card_id");
		builder.Property(x => x.ExternalId).HasColumnName("external_id");
		builder.Property(x => x.FileName).HasColumnName("file_name");
		builder.Property(x => x.LoadedDataTypeId).HasColumnName("loaded_data_type_id");

		builder.HasOne(x => x.Card).WithMany().HasForeignKey(x => x.CardId).OnDelete(DeleteBehavior.NoAction);
	}
}