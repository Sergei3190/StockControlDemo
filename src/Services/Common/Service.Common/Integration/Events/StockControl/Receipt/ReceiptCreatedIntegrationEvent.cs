namespace Service.Common.Integration.Events.StockControl.Receipt;

/// <summary>
/// Интеграционное событие создания поступления
/// </summary>
public class ReceiptCreatedIntegrationEvent : ProductFlowIntegrationEvent
{
	public ReceiptCreatedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
