using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using StockControl.API.Domain.Stock.Abstractions;

namespace StockControl.API.Domain.Stock;

// Тк у нас упрощённая версия складского учета, то в рамках одного вида движения товара добавляется одна номенклатура, а партия принадлежит и к
// движению товара и к номенклатуре, по хорошему, у Движения товара должна быть коллекция номенклатур, у каждой из которых создаётся свой id партии.

/// <summary>
/// Списание
/// </summary>
public class WriteOff : SendingProduct
{
	/// <summary>
	/// Причина
	/// </summary>
	public string? Reason { get; set; }

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
		$" {nameof(TotalPrice)}: {TotalPrice}" +
		$" {nameof(Reason)}: {Reason}";

	public class Map : IEntityTypeConfiguration<WriteOff>
	{
		public void Configure(EntityTypeBuilder<WriteOff> builder)
		{
			builder.ToTable("write_offs", "stock");

			builder.MapSendingProductEntity();

			builder.Property(x => x.Reason).HasColumnName("reason");

			builder.HasOne(x => x.ProductFlowType).WithMany(p => p.WriteOffs).HasForeignKey(x => x.ProductFlowTypeId).OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.SendingWarehouse).WithMany().HasForeignKey(x => x.SendingWarehouseId).OnDelete(DeleteBehavior.NoAction);

			builder.HasOne(x => x.Party).WithMany().HasForeignKey(i => i.PartyId).OnDelete(DeleteBehavior.NoAction);
		}
	}
}