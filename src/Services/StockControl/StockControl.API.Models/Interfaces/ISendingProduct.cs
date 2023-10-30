using Service.Common.DTO.Entities.Base;

namespace StockControl.API.Models.Interfaces;

/// <summary>
/// Отправитель товара
/// </summary>
public interface ISendingProduct : IProduct
{
	/// <summary>
	/// Склад отправитель
	/// </summary>
	public NamedEntityDto? SendingWarehouse { get; set; }
}