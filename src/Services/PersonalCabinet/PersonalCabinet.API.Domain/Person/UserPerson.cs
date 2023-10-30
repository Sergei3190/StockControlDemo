using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Converters.DB;
using Service.Common.Entities.App;
using Service.Common.Entities.Base;

namespace PersonalCabinet.API.Domain.Person;

/// <summary>
/// Персона пользователя
/// </summary>
public class UserPerson : MonitoringEntity
{
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public int? Age { get; set; }
    public DateOnly? Birthday { get; set; }

    //один к одному
    public Guid CardId { get; set; }
    public Card Card { get; set; }

    //один ко многим
    public Guid UserId { get; set; }
    public UserInfo User { get; set; }

    public override string ToString() => $"({nameof(Id)}: {Id}):" +
        $"{nameof(LastName)}: {LastName}" +
        $"{nameof(FirstName)}: {FirstName}" +
        $"{nameof(MiddleName)}: {MiddleName}" +
        $"{nameof(Age)}: {Age}" +
        $"{nameof(Birthday)}: {Birthday}" +
        $"{nameof(CardId)}: {CardId}" +
        $"{nameof(UserId)}: {UserId}";

    public class Map : IEntityTypeConfiguration<UserPerson>
    {
        public void Configure(EntityTypeBuilder<UserPerson> builder)
        {
            builder.ToTable("persons", "person");

            builder.MapMonitoringEntity();

            builder.Property(x => x.LastName).HasColumnName("last_name").IsRequired();
            builder.Property(x => x.FirstName).HasColumnName("first_name");
            builder.Property(x => x.MiddleName).HasColumnName("middle_name");
            builder.Property(x => x.Age).HasColumnName("age");
            builder.Property(x => x.Birthday).HasColumnName("birthday").HasConversion<DateOnlyConverter>().HasColumnType("date");
            builder.Property(x => x.CardId).HasColumnName("card_id");
            builder.Property(x => x.UserId).HasColumnName("user_id");

            builder.HasOne(x => x.Card).WithOne().HasForeignKey<UserPerson>(u => u.CardId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}