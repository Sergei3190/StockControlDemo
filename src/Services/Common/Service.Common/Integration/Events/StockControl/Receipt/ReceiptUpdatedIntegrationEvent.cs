namespace Service.Common.Integration.Events.StockControl.Receipt;

/// <summary>
/// Интеграционное событие обновления поступления
/// </summary>
public class ReceiptUpdatedIntegrationEvent : ProductFlowIntegrationEvent
{
	public ReceiptUpdatedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
