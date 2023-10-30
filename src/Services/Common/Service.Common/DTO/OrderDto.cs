namespace Service.Common.DTO;

/// <summary>
/// Модель для сортировки элементов в разрезе колонок бд
/// </summary>
public class OrderDto
{
	/// <summary>
	/// Наименование колонки в бд
	/// </summary>
	public string Column { get; set; }

	/// <summary>
	/// Направление сортировки
	/// </summary>
	public string Direction { get; set; }
}