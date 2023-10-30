namespace Service.Common.Integration.Events.StockControl.Receipt;

/// <summary>
/// Интеграционное событие удаления поступления
/// </summary>
public class ReceiptDeletedIntegrationEvent : ProductFlowIntegrationEvent
{
	public ReceiptDeletedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
