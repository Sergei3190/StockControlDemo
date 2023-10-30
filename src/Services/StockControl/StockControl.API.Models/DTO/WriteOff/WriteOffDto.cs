using Service.Common.DTO.Entities.Base;

using StockControl.API.Models.DTO.Party;
using StockControl.API.Models.Interfaces;

namespace StockControl.API.Models.DTO.WriteOff;

/// <summary>
/// Списание
/// </summary>
public class WriteOffDto : EntityDto, ISendingProduct, IProductFlowInfo
{
	private TimeOnly _createTime;

	public NamedEntityDto? ProductFlowType { get; set; }
	public string Number { get; set; }
	public DateOnly CreateDate { get; set; }

	public TimeOnly CreateTime
	{
		get => _createTime;
		set => this._createTime = new TimeOnly(value.Hour, value.Minute);
	}

	public PartyDto Party { get; set; }
	public NamedEntityDto Nomenclature { get; set; }
	public NamedEntityDto Warehouse { get; set; }
	public NamedEntityDto Organization { get; set; }
	public decimal Price { get; set; }
	public int Quantity { get; set; }
	public decimal? TotalPrice => Quantity * Price;
	public NamedEntityDto? SendingWarehouse { get; set; }

	/// <summary>
	/// Причина
	/// </summary>
	public string? Reason { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(ProductFlowType)}: {ProductFlowType.Id} {ProductFlowType?.Name}" +
		$" {nameof(Number)}: {Number} " +
		$" {nameof(CreateDate)}: {CreateDate}" +
		$" {nameof(CreateTime)}: {CreateTime}" +
		$" {nameof(Party)}: {Party?.Number} {Party?.CreateDate} {Party?.CreateTime}" +
		$" {nameof(Organization)}: {Organization.Id} {Organization?.Name}" +
		$" {nameof(Warehouse)}: {Warehouse.Id} {Warehouse?.Name}" +
		$" {nameof(SendingWarehouse)}: {SendingWarehouse?.Id} {SendingWarehouse?.Name}" +
		$" {nameof(Nomenclature)}: {Nomenclature.Id} {Nomenclature?.Name}" +
		$" {nameof(Price)}: {Price}" +
		$" {nameof(Quantity)}: {Quantity}" +
		$" {nameof(TotalPrice)}: {TotalPrice}" +
		$" {nameof(Reason)}: {Reason}";
}