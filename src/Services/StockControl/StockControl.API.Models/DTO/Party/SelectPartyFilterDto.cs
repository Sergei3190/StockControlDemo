using Service.Common.DTO;

using StockControl.API.Models.Interfaces.Filters;

namespace StockControl.API.Models.DTO;

/// <summary>
/// Фильтр партии товара для выпадающего списка
/// </summary>
public class SelectPartyFilterDto : FilterDto, ISelectPartyFilter
{
	public Guid? NomenclatureId { get; set; }
	public Guid? WarehouseId { get; set; }
	public Guid? OrganizationId { get; set; }
}