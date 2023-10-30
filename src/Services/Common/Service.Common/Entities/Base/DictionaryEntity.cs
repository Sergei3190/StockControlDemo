using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.Entities.Base;

public abstract class DictionaryEntity : NamedEntity, IDictionaryEntity
{
    public string? Mnemo { get; set; }
    public bool IsActive { get; set; }

    public override string ToString() => $"({nameof(Id)}: {Id}):" +
        $"{nameof(Name)}: {Name}" +
        $"{nameof(Mnemo)}: {Mnemo}" +
        $"{nameof(IsActive)}: {IsActive}";
}

public static class DictionaryConfiguraton
{
    public static void MapDictionaryEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : DictionaryEntity
    {
        builder.MapNamedEntity();

        builder.Property(x => x.Mnemo).HasColumnName("mnemo");
        builder.Property(x => x.IsActive).HasColumnName("is_active");
    }
}