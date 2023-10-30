using Service.Common.DTO;
using Service.Common.Interfaces;

using StockControl.API.Models.Interfaces.Filters;

namespace StockControl.API.Models.DTO.StockAvailability;

/// <summary>
/// Фильтр наличия товара на складах
/// </summary>
public class StockAvailabilityFilterDto : FilterDto, IProductFilter, IOrderByFilter
{
	public Guid? PartyId { get; set; }
	public Guid? NomenclatureId { get; set; }
	public Guid? WarehouseId { get; set; }
	public Guid? OrganizationId { get; set; }
	public OrderDto? Order { get; set; }

	// фильтрацию по указанным ниже свойствам на клиенте не буду реализовывать, тк приложение тестовое, принцип аналогичный с 
	// вышеуказанными свойствами : нужно сделать интерфейсы и к ним сервисы по выборке списков поступлений, списаний, перемещений, а также
	// учесть уже выбранные поля
	//public Guid? ReceiptId { get; set; }
	//public Guid? MovingId { get; set; }
	//public Guid? WriteOffId { get; set; }
}