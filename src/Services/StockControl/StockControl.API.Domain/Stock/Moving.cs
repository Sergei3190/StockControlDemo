using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StockControl.API.Domain.Stock.Abstractions;

namespace StockControl.API.Domain.Stock;

// Тк у нас упрощённая версия складского учета, то в рамках одного вида движения товара добавляется одна номенклатура, а партия принадлежит и к
// движению товара и к номенклатуре, по хорошему, у Движения товара должна быть коллекция номенклатур, у каждой из которых создаётся свой id партии.

/// <summary>
/// Перемещение
/// </summary>
public class Moving : SendingProduct
{
	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(ProductFlowTypeId)}: {ProductFlowTypeId}" +
		$" {nameof(Number)}: {Number} " +
		$" {nameof(CreateDate)}: {CreateDate}" +
		$" {nameof(CreateTime)}: {CreateTime}" +
		$" {nameof(PartyId)}: {PartyId}" +
		$" {nameof(OrganizationId)}: {OrganizationId}" +
		$" {nameof(WarehouseId)}: {WarehouseId}" +
		$" {nameof(SendingWarehouseId)}: {SendingWarehouseId}" +
		$" {nameof(NomenclatureId)}: {NomenclatureId} " +
		$" {nameof(Price)}: {Price}" +
		$" {nameof(Quantity)}: {Quantity}" +
		$" {nameof(TotalPrice)}: {TotalPrice}";

	public class Map : IEntityTypeConfiguration<Moving>
	{
		public void Configure(EntityTypeBuilder<Moving> builder)
		{
			builder.ToTable("movings", "stock");

			builder.MapSendingProductEntity();

			builder.HasOne(x => x.ProductFlowType).WithMany(p => p.Movings).HasForeignKey(x => x.ProductFlowTypeId).OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.SendingWarehouse).WithMany().HasForeignKey(x => x.SendingWarehouseId).OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.Party).WithMany().HasForeignKey(i => i.PartyId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}