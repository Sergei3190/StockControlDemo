namespace StockControl.API.Services.Interfaces;

/// <summary>
/// Сервис вспомогательной информации движения товаров
/// </summary>
public interface IProductFlowInfoService
{
    /// <summary>
    /// Получить задействованное кол-во движения товара
    /// </summary>
    Task<IEnumerable<(Guid ItemId, int InvolvedQuantity)>> GetInvolvedQuantityAsync(params (Guid Id, Guid PartyId, Guid WarehouseId)[] dtos);

    /// <summary>
    /// Получить данные для проверки задействованного кол-ва движения товара
    /// </summary>
    Task<IEnumerable<(Guid Id, Guid PartyId, string Number, Guid WarehouseId)>> GetCheckingDataAsync(params Guid[] ids);

    /// <summary>
    /// Получить данные для интеграционных событий
    /// </summary>
    Task<IEnumerable<(Guid Id, string Number)>> GetIntegrationData(params Guid[] ids);
}