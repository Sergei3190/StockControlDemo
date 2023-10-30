using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace Service.Common.Entities.App;

/// <summary>
/// Краткая информация о пользователе, поступающая из шины RabbitMQ
/// </summary>
public class UserInfo : NamedEntity
{
    public string Email { get; set; }

    public Guid SourceId { get; set; }
    public Source Source { get; set; }

    /// <summary>
    /// Признак блокировки пользователя
    /// </summary>
    public bool IsLockout { get; set; }

    public override string ToString() => $"({nameof(Id)}: {Id}):" +
        $"{nameof(Name)}: {Name}" +
        $"{nameof(Email)}: {Email} " +
        $"{nameof(SourceId)}: {SourceId}" +
        $"{nameof(IsLockout)}: {IsLockout}";

    public class Map : IEntityTypeConfiguration<UserInfo>
    {
        public void Configure(EntityTypeBuilder<UserInfo> builder)
        {
            builder.ToTable("users_info", "app");

            builder.Property(e => e.Email).HasColumnName("email");
            builder.Property(e => e.SourceId).HasColumnName("source_id");
            builder.Property(t => t.IsLockout).HasColumnName("is_lockout").HasDefaultValue(false);

            builder.HasOne(e => e.Source).WithMany(c => c.UsersInfo).HasForeignKey(e => e.SourceId).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(e => e.Name).IsUnique();
            builder.HasIndex(e => e.Email).IsUnique();

            builder.MapNamedEntity();
        }
    }
}