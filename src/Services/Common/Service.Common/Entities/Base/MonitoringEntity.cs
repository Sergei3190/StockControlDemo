using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Common.Entities.Base.Interfaces;
using Service.Common.Entities.App;

namespace Service.Common.Entities.Base;

public abstract class MonitoringEntity : Entity, IMonitoringEntity
{
    public DateTimeOffset CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }
    public UserInfo? CreatedByUser { get; set; }

    public DateTimeOffset? UpdatedDate { get; set; }

    public Guid? UpdatedBy { get; set; }
    public UserInfo? UpdatedByUser { get; set; }

    public DateTimeOffset? DeletedDate { get; set; }

    public Guid? DeletedBy { get; set; }
    public UserInfo? DeletedByUser { get; set; }
}

public static class MonitoringConfiguraton
{
    public static void MapMonitoringEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : MonitoringEntity
    {
        builder.MapEntity();

        builder.Property(t => t.CreatedDate).HasColumnName("_created").HasDefaultValue(DateTimeOffset.Now.ToLocalTime());
        builder.Property(t => t.CreatedBy).HasColumnName("_created_by");
        builder.Property(t => t.UpdatedDate).HasColumnName("_updated");
        builder.Property(t => t.UpdatedBy).HasColumnName("_updated_by");
        builder.Property(t => t.DeletedDate).HasColumnName("_deleted");
        builder.Property(t => t.DeletedBy).HasColumnName("_deleted_by");

        builder.HasOne(x => x.CreatedByUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.UpdatedByUser).WithMany().HasForeignKey(x => x.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.DeletedByUser).WithMany().HasForeignKey(x => x.DeletedBy).OnDelete(DeleteBehavior.NoAction);
    }
}