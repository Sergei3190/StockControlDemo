using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.Entities.Base;

public abstract class NamedEntity : Entity, INamedEntity
{
    public string Name { get; set; }
}

public static class NamedConfiguraton
{
    public static void MapNamedEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : NamedEntity
    {
        builder.MapEntity();

        builder.Property(x => x.Name).HasColumnName("name").IsRequired();
    }
}