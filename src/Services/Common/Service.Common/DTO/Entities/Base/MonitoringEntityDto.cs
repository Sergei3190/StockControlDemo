using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.DTO.Entities.Base;

public abstract class MonitoringEntityDto : EntityDto, IMonitoringEntity
{
	public DateTimeOffset CreatedDate { get; set; }
	public Guid? CreatedBy { get; set; }
	public DateTimeOffset? UpdatedDate { get; set; }
	public Guid? UpdatedBy { get; set; }
	public DateTimeOffset? DeletedDate { get; set; }
	public Guid? DeletedBy { get; set; }
}