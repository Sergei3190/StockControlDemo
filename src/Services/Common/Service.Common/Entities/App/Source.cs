using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace Service.Common.Entities.App;

/// <summary>
/// Внешние источники получения данных, например RabbitMQ
/// </summary>
public class Source : DictionaryEntity
{
    // сразу инициализаруем в бд при запуске приложения, нужно для безопасности, но больше для информативности того, откуда данные пришли в бд
    public static Source[] Sources = new[]
    {
        new Source()
        {
            Id = Guid.Parse("73F19E6B-73AD-462B-AC5D-0F3464815314"),
            Name = "RabbitMQ Складского Учета",
            Mnemo = "Stock Contol RabbitMQ",
            IsActive = true,
        }
    };

    public Source()
    {
        UsersInfo = new HashSet<UserInfo>();
    }

    public ICollection<UserInfo> UsersInfo { get; set; }

    public class Map : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            builder.ToTable("sources", "app");

            builder.MapDictionaryEntity();

            builder.HasMany(s => s.UsersInfo).WithOne(u => u.Source).HasForeignKey(u => u.SourceId);
        }
    }
}