using Service.Common.DTO;

using StockControl.API.Models.Interfaces.Filters;

namespace StockControl.API.Models.DTO.Warehouse;

/// <summary>
/// Фильтр элементов справочника склады для выпадающего списка
/// </summary>
public class SelectWarehouseFilterDto : FilterDto, ISelectWarehouseFilter
{
	public Guid? PartyId { get; set; }
	public Guid? NomenclatureId { get; set; }
	public Guid? OrganizationId { get; set; }
}