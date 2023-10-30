namespace StockControl.API.Models.Interfaces.Filters;

/// <summary>
/// Фильтр партий поступления товара для выпадающего списка
/// </summary>
public interface ISelectPartyFilter
{
	public Guid? NomenclatureId { get; set; }
	public Guid? WarehouseId { get; set; }
	public Guid? OrganizationId { get; set; }
}