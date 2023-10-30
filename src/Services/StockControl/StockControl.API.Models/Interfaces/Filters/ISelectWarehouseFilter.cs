namespace StockControl.API.Models.Interfaces.Filters;

/// <summary>
/// Фильтр элементов справочника склады для выпадающего списка
/// </summary>
public interface ISelectWarehouseFilter
{
	public Guid? PartyId { get; set; }
	public Guid? NomenclatureId { get; set; }
	public Guid? OrganizationId { get; set; }
}