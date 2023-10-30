using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.DTO.Party;

namespace StockControl.API.Models.Interfaces;

/// <summary>
/// Товар
/// </summary>
public interface IProduct
{
	/// <summary>
	/// Идентификатор партии (формируется при сохранении поступления и дальше кочует по всем списаниям и перемещениям) 
	/// </summary>
	public PartyDto Party { get; set; }

	/// <summary>
	/// Номенклатура
	/// </summary>
	public NamedEntityDto Nomenclature { get; set; }

	/// <summary>
	/// Склад хранения/получатель
	/// </summary>
	public NamedEntityDto Warehouse { get; set; }

	/// <summary>
	/// Организация (поставщик)
	/// </summary>
	public NamedEntityDto Organization { get; set; }

	/// <summary>
	/// Прайс
	/// </summary>
	public decimal Price { get; set; }

	/// <summary>
	/// Количество товара
	/// </summary>
	public int Quantity { get; set; }

	/// <summary>
	/// Итоговая цена
	/// </summary>
	public decimal? TotalPrice { get; }
}