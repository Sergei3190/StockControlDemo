using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace Notification.API.Domain.Notice;

/// <summary>
/// Типы уведомлений
/// </summary>
public class NotificationType : DictionaryEntity
{
    public static NotificationType[] NotificationTypes = new[]
    {
        new NotificationType()
        {
            Id = Guid.Parse("9B5B27B2-513E-4129-ABE3-7341EDDF5B57"),
            Name = "Поступления",
            Mnemo = "Receipts",
            IsActive = true,
        },
        new NotificationType()
        {
            Id = Guid.Parse("D16346B3-5119-4B65-827C-3613BD811616"),
            Name = "Перемещения",
            Mnemo = "Movings",
            IsActive = true,
        },
        new NotificationType()
        {
            Id = Guid.Parse("000E19EF-CC3C-4994-BC34-8BCC143169BF"),
            Name = "Списания",
            Mnemo = "WriteOffs",
            IsActive = true,
        }
    };

    public NotificationType()
    {
        Notifications = new HashSet<UserNotification>();
        NotificationSettings = new HashSet<NotificationSetting>();
    }

    public ICollection<UserNotification> Notifications { get; set; }
    public ICollection<NotificationSetting> NotificationSettings { get; set; }

    public class Map : IEntityTypeConfiguration<NotificationType>
    {
        public void Configure(EntityTypeBuilder<NotificationType> builder)
        {
            builder.ToTable("notification_types", "notice");

            builder.MapDictionaryEntity();

            builder.HasMany(i => i.Notifications).WithOne(x => x.NotificationType).HasForeignKey(x => x.NotificationTypeId);
            builder.HasMany(i => i.NotificationSettings).WithOne(x => x.NotificationType).HasForeignKey(x => x.NotificationTypeId);
        }
    }
}