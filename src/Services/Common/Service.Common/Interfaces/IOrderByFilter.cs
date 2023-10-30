using Service.Common.DTO;

namespace Service.Common.Interfaces;

/// <summary>
/// Фильтр серверной сортировки в разрезе колонок бд
/// </summary>
public interface IOrderByFilter
{
	public OrderDto? Order { get; set; }
}
