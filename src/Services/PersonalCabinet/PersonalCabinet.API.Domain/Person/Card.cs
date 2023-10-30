using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace PersonalCabinet.API.Domain.Person;

/// <summary>
/// Карта, которая идентифицирует персону, по ней можно получить все данные, относящиеся к конкретной персоне (создается при создании персоны, а персона создается
/// при регистрации пользователя, сразу как приходит информация из шины сообщения)
/// </summary>
public class Card : Entity
{
    public override string ToString() => $"({nameof(Id)}: {Id})";

    public class Map : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("cards", "person");

            builder.MapEntity();
        }
    }
}