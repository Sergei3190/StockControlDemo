namespace StockControl.API.Models.Interfaces.Filters;

/// <summary>
/// Фильтр элементов справочника организации для выпадающего списка
/// </summary>
public interface ISelectOrganizationFilter
{
    public Guid? PartyId { get; set; }
    public Guid? WarehouseId { get; set; }
	public Guid? NomenclatureId { get; set; }
}