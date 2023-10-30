namespace Service.Common.Entities.Base.Interfaces;

public interface IDictionaryEntity : INamedEntity
{
    public string? Mnemo { get; set; }
    public bool IsActive { get; set; }
}