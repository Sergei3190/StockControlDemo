namespace StockControl.API.Models.Interfaces.Filters;

/// <summary>
/// Фильтр отправителя товара
/// </summary>
public interface ISendingProductFilter : IProductFilter
{
    public Guid? SendingWarehouseId { get; set; }
}