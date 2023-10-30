using Service.Common.DTO.Entities.Base;

namespace StockControl.API.Models.DTO.Classifier;

/// <summary>
/// Справочник
/// </summary>
public class ClassifierDto : NamedEntityDto
{
	/// <summary>
	/// Путь справочника, нужен для роутинга в UI
	/// </summary>
	public string Path { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$"{nameof(Name)}: {Name}" +
		$"{nameof(Path)}: {Path}";
}