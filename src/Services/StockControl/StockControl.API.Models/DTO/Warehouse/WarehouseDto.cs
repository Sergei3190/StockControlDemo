using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.Interfaces;

namespace StockControl.API.Models.DTO.Warehouse;

/// <summary>
/// Склад
/// </summary>
public class WarehouseDto : NamedEntityDto, IClassifierItem
{
	public NamedEntityDto? Classifier { get; set; }

	// Здесь дополнимтельные поля справочника

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$"{nameof(Name)}: {Name}" +
		$"{nameof(Classifier)}: {Classifier?.Id} {Classifier?.Name}";
}