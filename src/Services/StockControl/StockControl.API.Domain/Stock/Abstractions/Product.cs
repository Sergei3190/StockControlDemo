using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Entities.Base;

namespace StockControl.API.Domain.Stock.Abstractions;

/// <summary>
/// Товар
/// </summary>
public abstract class Product: MonitoringEntity
{
	/// <summary>
	/// Идентификатор партии (формируется при сохранении поступления и дальше кочует по всем списаниям и перемещениям) 
	/// </summary>
	public Guid PartyId { get; set; }
	public Party Party { get; set; }

    /// <summary>
    /// Номенклатура
    /// </summary>
    public Guid NomenclatureId { get; set; }
    public Nomenclature Nomenclature { get; set; }

    /// <summary>
    /// Склад хранения/получатель
    /// </summary>
    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; }

    /// <summary>
    /// Организация (поставщик)
    /// </summary>
    public Guid OrganizationId { get; set; }
    public Organization Organization { get; set; }

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
    public decimal? TotalPrice { get; set; }

    public override string ToString() => $"({nameof(Id)}: {Id}):" +
        $" {nameof(PartyId)}: {PartyId}" +
        $" {nameof(OrganizationId)}: {OrganizationId}" +
        $" {nameof(WarehouseId)}: {WarehouseId}" +
        $" {nameof(NomenclatureId)}: {NomenclatureId} " +
        $" {nameof(Price)}: {Price}" +
        $" {nameof(Quantity)}: {Quantity}" +
        $" {nameof(TotalPrice)}: {TotalPrice}";
}

public static class ProductConfiguraton
{
    public static void MapProductEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : Product
    {
        builder.MapMonitoringEntity();

        builder.Property(x => x.PartyId).HasColumnName("party_id");
        builder.Property(x => x.NomenclatureId).HasColumnName("nomenclature_id");
        builder.Property(x => x.WarehouseId).HasColumnName("warehouse_id");
        builder.Property(x => x.OrganizationId).HasColumnName("organization_id");

        builder.Property(x => x.Price).HasColumnName("price").HasColumnType("decimal(18,2)");
        builder.Property(x => x.Quantity).HasColumnName("quantity").IsRequired().HasColumnOrder(8);
        builder.Property(x => x.TotalPrice).HasColumnName("total_price").HasColumnType("decimal(18,2)");

        builder.HasOne(x => x.Organization).WithMany().HasForeignKey(i => i.OrganizationId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Warehouse).WithMany().HasForeignKey(i => i.WarehouseId).OnDelete(DeleteBehavior.NoAction);
        builder.HasOne(x => x.Nomenclature).WithMany().HasForeignKey(i => i.NomenclatureId).OnDelete(DeleteBehavior.NoAction);
    }
}