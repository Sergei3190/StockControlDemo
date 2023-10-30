using Service.Common.DTO;
using Service.Common.Interfaces;

using StockControl.API.Models.Interfaces.Filters;

namespace StockControl.API.Models.DTO.Moving;

/// <summary>
/// Фильтр перемещений
/// </summary>
public class MovingFilterDto : FilterDto, ISendingProductFilter, IOrderByFilter
{
	public Guid? PartyId { get; set; }
	public Guid? NomenclatureId { get; set; }
	public Guid? WarehouseId { get; set; }
	public Guid? OrganizationId { get; set; }
	public Guid? SendingWarehouseId { get; set; }
	public OrderDto? Order { get; set; }
}