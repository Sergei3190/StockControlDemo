namespace Service.Common.Entities.Base.Interfaces;

public interface INamedEntity : IEntity
{
    public string Name { get; set; }
}