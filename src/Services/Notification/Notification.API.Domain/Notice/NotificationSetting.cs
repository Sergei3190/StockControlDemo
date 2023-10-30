using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.App;
using Service.Common.Entities.Base;

namespace Notification.API.Domain.Notice;

/// <summary>
/// Настройки уведомления (заполняется тригером при получении из шины сообщений данных о пользователе)
/// </summary>
public class NotificationSetting : MonitoringEntity
{
    /// <summary>
    /// Тип уведомления
    /// </summary>
    public Guid NotificationTypeId { get; set; }
    public NotificationType NotificationType { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public Guid UserId { get; set; }
    public UserInfo User { get; set; }

    /// <summary>
    /// Признак включенного уведомления
    /// </summary>
    public bool Enable { get; set; }

    public override string ToString() => $"({nameof(Id)}: {Id}):" +
        $" {nameof(NotificationTypeId)}: {NotificationTypeId}" +
        $" {nameof(UserId)}: {UserId}" +
        $" {nameof(Enable)}: {Enable}";

    public class Map : IEntityTypeConfiguration<NotificationSetting>
    {
        public void Configure(EntityTypeBuilder<NotificationSetting> builder)
        {
            builder.ToTable("notification_settings", "notice");

            builder.MapMonitoringEntity();

            builder.Property(x => x.NotificationTypeId).HasColumnName("notification_type_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");
            builder.Property(x => x.Enable).HasColumnName("enable");

            builder.HasOne(x => x.NotificationType).WithMany(n => n.NotificationSettings).HasForeignKey(x => x.NotificationTypeId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);

            // для каждого пользователя должна быть одна настройка одного типа
            builder.HasIndex(x => new { x.NotificationTypeId, x.UserId }).IsUnique();
        }
    }
}