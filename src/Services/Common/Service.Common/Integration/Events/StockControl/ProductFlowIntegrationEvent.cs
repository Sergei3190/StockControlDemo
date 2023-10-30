using Service.Common.Integration.Events.Base;

namespace Service.Common.Integration.Events.StockControl;

/// <summary>
/// Базовое интеграционное событие движения товара
/// </summary>
public abstract class ProductFlowIntegrationEvent : SCRMQIntegrationEvent
{
	public Guid ProductFlowId { get; init; }
	public string Number { get; init; }
	public string CreatorName { get; init; }

	public ProductFlowIntegrationEvent(Guid productFlowId, string number, string creatorName)
	{
		ProductFlowId = productFlowId;
		Number = number;
		CreatorName = creatorName;
	}
}
