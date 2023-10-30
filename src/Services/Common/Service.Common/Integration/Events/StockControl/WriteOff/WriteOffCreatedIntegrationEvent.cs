namespace Service.Common.Integration.Events.StockControl.WriteOff;

/// <summary>
/// Интеграционное событие создания списания
/// </summary>
public class WriteOffCreatedIntegrationEvent : ProductFlowIntegrationEvent
{
	public WriteOffCreatedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
