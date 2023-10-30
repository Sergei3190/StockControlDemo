using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Converters.DB;
using Service.Common.Entities.App;
using Service.Common.Entities.Base;

namespace Note.API.Domain.Note;

/// <summary>
/// Заметка
/// </summary>
public class UserNote : MonitoringEntity 
{
    /// <summary>
    /// Содержание
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Признак зафиксированной заметки
    /// </summary>
    public bool IsFix { get; set; }

    /// <summary>
    /// Номер сортировки
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// Дата выполнения заметки
    /// </summary>
    public DateOnly? ExecutionDate { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public Guid UserId { get; set; }
    public UserInfo User { get; set; }

    public override string ToString() => $"({nameof(Id)}: {Id}):" +
        $" {nameof(Content)}: {Content} " +
        $" {nameof(IsFix)}: {IsFix}" +
        $" {nameof(Sort)}: {Sort}" +
        $" {nameof(ExecutionDate)}: {ExecutionDate}" +
        $" {nameof(UserId)}: {UserId}";

    public class Map : IEntityTypeConfiguration<UserNote>
    {
        public void Configure(EntityTypeBuilder<UserNote> builder)
        {
            builder.ToTable("notes", "note");

            builder.MapMonitoringEntity();

            builder.Property(x => x.Content).HasColumnName("content").IsRequired();
            builder.Property(x => x.IsFix).HasColumnName("is_fix");
            builder.Property(x => x.Sort).HasColumnName("sort");
            builder.Property(x => x.ExecutionDate).HasColumnName("execution_date").HasConversion<DateOnlyConverter>().HasColumnType("date");
            builder.Property(x => x.UserId).HasColumnName("user_id");

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);

            builder.HasIndex(x => x.Content);
        }
    }
}