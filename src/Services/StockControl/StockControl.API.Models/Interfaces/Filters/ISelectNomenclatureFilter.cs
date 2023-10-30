namespace StockControl.API.Models.Interfaces.Filters;

/// <summary>
/// Фильтр элементов справочника номенклатура для выпадающего списка
/// </summary>
public interface ISelectNomenclatureFilter
{
    public Guid? PartyId { get; set; }
    public Guid? WarehouseId { get; set; }
    public Guid? OrganizationId { get; set; }
}