namespace Service.Common.Integration.Events.StockControl.Moving;


/// <summary>
/// Интеграционное событие создания перемещения
/// </summary>
public class MovingCreatedIntegrationEvent : ProductFlowIntegrationEvent
{
	public MovingCreatedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
