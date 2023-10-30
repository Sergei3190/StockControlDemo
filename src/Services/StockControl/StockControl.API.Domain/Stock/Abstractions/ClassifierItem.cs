using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace StockControl.API.Domain.Stock.Abstractions;

/// <summary>
/// Элемент справочника
/// </summary>
public abstract class ClassifierItem : NamedMonitoringEntity
{
	public Guid ClassifierId { get; set; }
	public Classifier Classifier { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$"{nameof(Name)}: {Name}" +
		$"{nameof(ClassifierId)}: {ClassifierId}";
}

public static class ClassifierItemConfiguraton
{
	public static void MapClassifierItemEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : ClassifierItem
	{
		builder.MapNamedMonitoringEntity();

		builder.Property(x => x.ClassifierId).HasColumnName("classifier_id");

		builder.HasIndex(x => x.Name);
	}
}