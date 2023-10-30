using Service.Common.DTO;
using StockControl.API.Models.Interfaces.Filters;

namespace StockControl.API.Models.DTO.Organization;

/// <summary>
/// Фильтр элементов справочника организации для выпадающего списка
/// </summary>
public class SelectOrganizationFilterDto : FilterDto, ISelectOrganizationFilter
{
	public Guid? PartyId { get; set; }
	public Guid? WarehouseId { get; set; }
	public Guid? NomenclatureId { get; set; }
}