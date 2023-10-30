using Service.Common.DTO;
using Service.Common.Interfaces;

namespace StockControl.API.Models.DTO.Nomenclature;

/// <summary>
/// Фильтр номенклатуры
/// </summary>
public class NomenclatureFilterDto : FilterDto, IOrderByFilter
{
	public OrderDto? Order { get; set; }
}