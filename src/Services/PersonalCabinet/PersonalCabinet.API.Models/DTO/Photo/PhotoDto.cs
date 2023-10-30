using PersonalCabinet.API.Models.Interfaces;
using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.Models.DTO.Photo;

/// <summary>
/// Загруженное фото персоны
/// </summary>
public class PhotoDto : EntityDto, ILoadedData
{
	public Guid CardId { get; set; }

	public FileDto File { get; set; }

	public NamedEntityDto? LoadedDataType { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$"{nameof(CardId)}: {CardId}" +
		$"{nameof(File.FileName)}: {File.FileName}" +
		$"{nameof(LoadedDataType.Name)}: {LoadedDataType?.Name}";
}