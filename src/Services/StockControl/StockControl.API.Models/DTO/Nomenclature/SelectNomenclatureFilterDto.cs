using Service.Common.DTO;

using StockControl.API.Models.Interfaces.Filters;

namespace StockControl.API.Models.DTO.Nomenclature;

/// <summary>
/// Фильтр элементов справочника номенклатура 
/// </summary>
public class SelectNomenclatureFilterDto : FilterDto, ISelectNomenclatureFilter
{
	public Guid? PartyId { get; set; }
	public Guid? WarehouseId { get; set; }
	public Guid? OrganizationId { get; set; }
}