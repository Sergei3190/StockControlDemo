using Service.Common.DTO.Entities.Base;

namespace StockControl.API.Models.DTO.Party;

/// <summary>
/// Партия товара
/// </summary>
public class PartyDto : EntityDto
{
	// number + дата + время будут отображаться пользователю для выбора партии

	private TimeOnly? _createTime;

	/// <summary>
	/// Номер партии
	/// </summary>
	public string? Number { get; set; }

	/// <summary>
	/// Уникальный номер партии получателя 
	/// (генерируется на сервере при создании партии, нужно для UI, в случаи если приходят одни и те же партии изготовителя в разных поступлениях)
	/// </summary>
	public string? ExtensionNumber { get; set; }

	/// <summary>
	/// Дата изготовления
	/// </summary>
	public DateOnly? CreateDate { get; set; }

    /// <summary>
    /// Время изготовления
    /// </summary>
    public TimeOnly? CreateTime 
	{
		get => _createTime;
		set 
	    {
			if (value.HasValue)
				this._createTime = new TimeOnly(value.Value.Hour, value.Value.Minute);
			else
				this._createTime = null;
		}
	}
}