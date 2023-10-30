using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.DTO.Entities.Base;

public class NamedMonitoringEntityDto : MonitoringEntityDto, INamedMonitoringEntity
{
	public string Name { get; set; }
}