namespace Service.Common.Integration.Events.StockControl.WriteOff;

/// <summary>
/// Интеграционное событие удаления списания
/// </summary>
public class WriteOffDeletedIntegrationEvent : ProductFlowIntegrationEvent
{
	public WriteOffDeletedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
