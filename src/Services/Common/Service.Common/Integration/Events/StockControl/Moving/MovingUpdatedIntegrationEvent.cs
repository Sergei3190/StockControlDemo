namespace Service.Common.Integration.Events.StockControl.Moving;

/// <summary>
/// Интеграционное событие обновления перемещения
/// </summary>
public class MovingUpdatedIntegrationEvent : ProductFlowIntegrationEvent
{
	public MovingUpdatedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
