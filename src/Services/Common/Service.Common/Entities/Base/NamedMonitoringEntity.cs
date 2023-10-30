using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.Entities.Base;

public abstract class NamedMonitoringEntity : MonitoringEntity, INamedMonitoringEntity
{
    public string Name { get; set; }
}

public static class NamedMonitoringConfiguraton
{
    public static void MapNamedMonitoringEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : NamedMonitoringEntity
    {
        builder.MapMonitoringEntity();

        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
    }
}