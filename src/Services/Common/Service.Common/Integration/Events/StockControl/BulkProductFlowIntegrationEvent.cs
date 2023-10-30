using Service.Common.Integration.DTO;
using Service.Common.Integration.Events.Base;

namespace Service.Common.Integration.Events.StockControl;

/// <summary>
/// Базовое интеграционное событие массовых операций движения товара
/// </summary>
public abstract class BulkProductFlowIntegrationEvent : SCRMQIntegrationEvent
{
	public IEnumerable<ProductFlowInfoDto> Info { get; init; }
	public string CreatorName { get; init; }

	public BulkProductFlowIntegrationEvent(IEnumerable<ProductFlowInfoDto> info, string creatorName)
	{
		Info = info;
		CreatorName = creatorName;
	}
}