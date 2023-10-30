using Service.Common.DTO;
using Service.Common.Interfaces;

namespace StockControl.API.Models.DTO.Organization;

/// <summary>
/// Фильтр организаций
/// </summary>
public class OrganizationFilterDto : FilterDto, IOrderByFilter
{
	public OrderDto? Order { get; set; }
}