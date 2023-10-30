using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Converters.DB;
using Service.Common.Entities.App;
using Service.Common.Entities.Base;

namespace Notification.API.Domain.Notice;

/// <summary>
/// Уведомление
/// </summary>
public class UserNotification : MonitoringEntity
{
    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Тип уведомления
    /// </summary>
    public Guid NotificationTypeId { get; set; }
    public NotificationType NotificationType { get; set; }

    /// <summary>
    /// Признак отправленного уведомления
    /// </summary>
    public bool IsSend { get; set; }

    /// <summary>
    /// Дата отправки
    /// </summary>
    public DateOnly? SendDate { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public Guid UserId { get; set; }
    public UserInfo User { get; set; }

    public override string ToString() => $"({nameof(Id)}: {Id}):" +
        $" {nameof(Content)}: {Content} " +
        $" {nameof(NotificationTypeId)}: {NotificationTypeId}" +
        $" {nameof(IsSend)}: {IsSend}" +
        $" {nameof(SendDate)}: {SendDate}" +
        $" {nameof(UserId)}: {UserId}";

    public class Map : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("notifications", "notice");

            builder.MapMonitoringEntity();

            builder.Property(x => x.Content).HasColumnName("content").IsRequired();
            builder.Property(x => x.IsSend).HasColumnName("is_send");
            builder.Property(x => x.NotificationTypeId).HasColumnName("notification_type_id");
            builder.Property(x => x.SendDate).HasColumnName("send_date").HasConversion<DateOnlyConverter>().HasColumnType("date");
            builder.Property(x => x.UserId).HasColumnName("user_id");

            builder.HasOne(x => x.NotificationType).WithMany(n => n.Notifications).HasForeignKey(x => x.NotificationTypeId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.Content);
            builder.HasIndex(x => x.SendDate);
        }
    }
}