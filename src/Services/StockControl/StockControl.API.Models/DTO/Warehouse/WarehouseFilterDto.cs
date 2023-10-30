using Service.Common.DTO;
using Service.Common.Interfaces;

namespace StockControl.API.Models.DTO.Warehouse;

/// <summary>
/// Фильтр складов
/// </summary>
public class WarehouseFilterDto : FilterDto, IOrderByFilter
{
	public OrderDto? Order { get; set; }
}