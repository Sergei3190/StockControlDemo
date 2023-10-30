namespace StockControl.API.Services.Interfaces;

/// <summary>
/// Сервис получения информации о элементе справочника
/// </summary>
public interface IClassifierItemInfoService
{
	/// <summary>
	/// Получение коллекции номеров документов движения товара по илентификатору элемента справочника
	/// </summary>
	Task<IEnumerable<(Guid itemId, string name, string number)>> GetProductFlowNumbersByItemIdAsync(params Guid[] ids);
}
