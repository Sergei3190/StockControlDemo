using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StockControl.API.Domain.Stock.Abstractions;

namespace StockControl.API.Domain.Stock;

// Тк у нас упрощённая версия складского учета, то в рамках одного вида движения товара добавляется одна номенклатура, а партия принадлежит и к
// движению товара и к номенклатуре, по хорошему, у Движения товара должна быть коллекция номенклатур, у каждой из которых создаётся свой id партии.

/// <summary>
/// Поступление
/// </summary>
public class Receipt : ProductFlowInfo
{
	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(ProductFlowTypeId)}: {ProductFlowTypeId}" +
		$" {nameof(Number)}: {Number} " +
		$" {nameof(CreateDate)}: {CreateDate}" +
		$" {nameof(CreateTime)}: {CreateTime}" +
		$" {nameof(PartyId)}: {PartyId}" +
		$" {nameof(OrganizationId)}: {OrganizationId}" +
		$" {nameof(WarehouseId)}: {WarehouseId}" +
		$" {nameof(NomenclatureId)}: {NomenclatureId} " +
		$" {nameof(Price)}: {Price}" +
		$" {nameof(Quantity)}: {Quantity}" +
		$" {nameof(TotalPrice)}: {TotalPrice}";

	public class Map : IEntityTypeConfiguration<Receipt>
	{
		public void Configure(EntityTypeBuilder<Receipt> builder)
		{
			builder.ToTable("receipts", "stock");

			builder.MapProductFlowInfoEntity();

			builder.HasOne(x => x.ProductFlowType).WithMany(p => p.Receipts).HasForeignKey(x => x.ProductFlowTypeId).OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.Party).WithOne().HasForeignKey<Receipt>(u => u.PartyId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}