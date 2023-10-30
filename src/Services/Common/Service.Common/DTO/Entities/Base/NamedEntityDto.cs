using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.DTO.Entities.Base;

public class NamedEntityDto : EntityDto, INamedEntity
{
	public string Name { get; set; }
}