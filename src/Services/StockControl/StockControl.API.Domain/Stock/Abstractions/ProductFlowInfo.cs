using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Service.Common.Converters.DB;

namespace StockControl.API.Domain.Stock.Abstractions;

/// <summary>
/// Основная информация о движении товара
/// </summary>
public abstract class ProductFlowInfo : Product
{
	/// <summary>
	/// Тип движения (поступление/перемещение/списание)
	/// </summary>
	public Guid ProductFlowTypeId { get; set; }
	public ProductFlowType ProductFlowType { get; set; }

	/// <summary>
	/// Номер поступления/перемещения/списания
	/// </summary>
	public string Number { get; set; }

	/// <summary>
	/// Дата поступления/перемещения/списания
	/// </summary>
	public DateOnly CreateDate { get; set; }

	/// <summary>
	/// Время поступления/перемещения/списания
	/// </summary>
	public TimeOnly CreateTime { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$" {nameof(ProductFlowType)}: {ProductFlowType.Id} {ProductFlowType?.Name}" +
		$" {nameof(Number)}: {Number} " +
		$" {nameof(CreateDate)}: {CreateDate}" +
		$" {nameof(CreateTime)}: {CreateTime}" +
		$" {nameof(PartyId)}: {PartyId}" +
		$" {nameof(Organization)}: {Organization.Id} {Organization?.Name}" +
		$" {nameof(Warehouse)}: {Warehouse.Id} {Warehouse?.Name}" +
		$" {nameof(Nomenclature)}: {Nomenclature.Id} {Nomenclature?.Name}" +
		$" {nameof(Price)}: {Price}" +
		$" {nameof(Quantity)}: {Quantity}" +
		$" {nameof(TotalPrice)}: {TotalPrice}";
}

public static class ProductFlowInfoConfiguraton
{
	public static void MapProductFlowInfoEntity<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : ProductFlowInfo
	{
		builder.MapProductEntity();

		builder.Property(x => x.Number).HasColumnName("number").IsRequired();
		builder.Property(x => x.CreateDate).HasColumnName("create_date").HasConversion<DateOnlyConverter>().HasColumnType("date").IsRequired();
		builder.Property(x => x.CreateTime).HasColumnName("create_time").HasConversion<TimeOnlyConverter>().HasColumnType("time(0)").IsRequired();
		builder.Property(x => x.ProductFlowTypeId).HasColumnName("product_flow_type_id");

		// нужно для фильтрации продукции по партиям
		builder.HasIndex(x => new { x.ProductFlowTypeId, x.Number, x.CreateDate, x.CreateTime }).IsUnique();
	}
}