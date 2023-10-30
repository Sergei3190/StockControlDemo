namespace Service.Common.Entities.Base.Interfaces;

public interface INamedMonitoringEntity : IMonitoringEntity
{
    public string Name { get; set; }
}