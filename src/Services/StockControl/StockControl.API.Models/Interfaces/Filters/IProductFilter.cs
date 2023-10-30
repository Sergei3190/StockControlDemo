namespace StockControl.API.Models.Interfaces.Filters;

/// <summary>
/// Фильтр товара
/// </summary>
public interface IProductFilter
{
    public Guid? PartyId { get; set; }
    public Guid? NomenclatureId { get; set; }
    public Guid? WarehouseId { get; set; }
    public Guid? OrganizationId { get; set; }
}