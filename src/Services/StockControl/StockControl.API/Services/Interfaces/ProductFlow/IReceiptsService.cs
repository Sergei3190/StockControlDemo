using Service.Common.Interfaces;

using StockControl.API.Models.DTO.Receipt;

namespace StockControl.API.Services.Interfaces.ProductFlow;

/// <summary>
/// Сервис поступлений
/// </summary>
public interface IReceiptsService : ICrudService<ReceiptDto, ReceiptFilterDto>, IBulkDeleteService, IProductFlowInfoService
{
	/// <summary>
	/// Получить дату и время создания поступления по id партии
	/// </summary>
	Task<(DateOnly CreateDate, TimeOnly CreateTime)?> GetDataTimeCreatedByPartyIdAsync(Guid partyId);
}