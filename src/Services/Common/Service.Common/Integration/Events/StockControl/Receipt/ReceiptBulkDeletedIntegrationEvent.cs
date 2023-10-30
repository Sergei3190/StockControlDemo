using Service.Common.Integration.DTO;

namespace Service.Common.Integration.Events.StockControl.Receipt;

/// <summary>
/// Интеграционное событие массового удаления поступлений
/// </summary>
public class ReceiptBulkDeletedIntegrationEvent : BulkProductFlowIntegrationEvent
{
	public ReceiptBulkDeletedIntegrationEvent(IEnumerable<ProductFlowInfoDto> info, string creatorName)
		: base(info, creatorName)
	{
	}
}
