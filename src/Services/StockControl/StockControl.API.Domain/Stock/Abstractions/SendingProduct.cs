using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace StockControl.API.Domain.Stock.Abstractions;

/// <summary>
/// Отправитель товара
/// </summary>
public abstract class SendingProduct : ProductFlowInfo
{
	/// <summary>
	/// Склад отправитель
	/// </summary>
	public Guid? SendingWarehouseId { get; set; }
	public Warehouse? SendingWarehouse { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(ProductFlowType)}: {ProductFlowType.Id} {ProductFlowType?.Name}" +
		$" {nameof(Number)}: {Number} " +
		$" {nameof(CreateDate)}: {CreateDate}" +
		$" {nameof(CreateTime)}: {CreateTime}" +
		$" {nameof(PartyId)}: {PartyId}" +
		$" {nameof(Organization)}: {Organization.Id} {Organization?.Name}" +
		$" {nameof(SendingWarehouseId)}: {SendingWarehouseId}" +
		$" {nameof(Warehouse)}: {Warehouse.Id} {Warehouse?.Name}" +
		$" {nameof(Nomenclature)}: {Nomenclature.Id} {Nomenclature?.Name}" +
		$" {nameof(Price)}: {Price}" +
		$" {nameof(Quantity)}: {Quantity}" +
		$" {nameof(TotalPrice)}: {TotalPrice}";
}

public static class SendingProductConfiguraton
{
	public static void MapSendingProductEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : SendingProduct
	{
		builder.MapProductFlowInfoEntity();

		builder.Property(x => x.SendingWarehouseId).HasColumnName("sending_warehouse_id");
	}
}