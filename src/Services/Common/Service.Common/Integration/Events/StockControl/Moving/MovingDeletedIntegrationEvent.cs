namespace Service.Common.Integration.Events.StockControl.Moving;

/// <summary>
/// Интеграционное событие удаления перемещения
/// </summary>
public class MovingDeletedIntegrationEvent : ProductFlowIntegrationEvent
{
	public MovingDeletedIntegrationEvent(Guid productFlowId, string number, string creatorName)
		: base(productFlowId, number, creatorName)
	{

	}
}
