using Service.Common.DTO.Entities.Base;

namespace StockControl.API.Models.Interfaces;

/// <summary>
/// Основная информация о движении товара
/// </summary>
public interface IProductFlowInfo
{
	/// <summary>
	/// Тип движения (поступление/перемещение/списание)
	/// </summary>
	public NamedEntityDto? ProductFlowType { get; set; }

	/// <summary>
	/// Номер поступления/перемещения/списания
	/// </summary>
	public string Number { get; set; }

	/// <summary>
	/// Дата поступления/перемещения/списания
	/// </summary>
	public DateOnly CreateDate { get; set; }

	/// <summary>
	/// Время поступления/перемещения/списания
	/// </summary>
	public TimeOnly CreateTime { get; set; }
}