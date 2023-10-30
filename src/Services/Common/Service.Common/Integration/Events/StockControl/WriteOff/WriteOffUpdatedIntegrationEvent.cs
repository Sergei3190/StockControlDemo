namespace Service.Common.Integration.Events.StockControl.WriteOff;

/// <summary>
/// Интеграционное событие обновления списания
/// </summary>
public class WriteOffUpdatedIntegrationEvent : ProductFlowIntegrationEvent
{
	public WriteOffUpdatedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
