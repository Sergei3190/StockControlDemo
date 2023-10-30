using Service.Common.Entities.Base.Interfaces;

namespace Service.Common.DTO.Entities.Base;

public class DictionaryEntityDto : NamedEntityDto, IDictionaryEntity
{
	public string? Mnemo { get; set; }
	public bool IsActive { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$"{nameof(Name)}: {Name}" +
		$"{nameof(Mnemo)}: {Mnemo}" +
		$"{nameof(IsActive)}: {IsActive}";
}