using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.DTO.Party;
using StockControl.API.Models.Interfaces;

namespace StockControl.API.Models.DTO.StockAvailability;

/// <summary>
/// Наличие товара на складах (обновляется отдельным фоновым процессом, после каждого создания/обновления документа движения товара, внутри процесса устанавливаем lock)
/// </summary>
public class StockAvailabilityDto : EntityDto, IProduct
{
	public PartyDto Party { get; set; }
	public NamedEntityDto Nomenclature { get; set; }
	public NamedEntityDto Warehouse { get; set; }
	public NamedEntityDto Organization { get; set; }
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public decimal? TotalPrice => Quantity * Price;

	/// <summary>
	/// Поступление
	/// </summary>
	public Guid? ReceiptId { get; set; }

	/// <summary>
	/// Перемещение
	/// </summary>
	public Guid? MovingId { get; set; }

	/// <summary>
	/// Списание
	/// </summary>
	public Guid? WriteOffId { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(Party)}: {Party?.Number} {Party?.CreateDate} {Party?.CreateTime}" +
		$" {nameof(Organization)}: {Organization.Id} {Organization?.Name}" +
		$" {nameof(Warehouse)}: {Warehouse.Id} {Warehouse?.Name}" +
		$" {nameof(Nomenclature)}: {Nomenclature.Id} {Nomenclature?.Name}" +
		$" {nameof(Price)}: {Price}" +
		$" {nameof(Quantity)}: {Quantity}" +
		$" {nameof(TotalPrice)}: {TotalPrice}";
}