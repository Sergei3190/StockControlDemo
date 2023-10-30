using Service.Common.DTO.Entities.Base;

namespace StockControl.API.Models.Interfaces;

/// <summary>
/// Элемент справочника
/// </summary>
public interface IClassifierItem
{
	public NamedEntityDto? Classifier { get; set; }
}