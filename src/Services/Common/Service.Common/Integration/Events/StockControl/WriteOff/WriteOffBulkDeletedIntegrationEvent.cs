using Service.Common.Integration.DTO;

namespace Service.Common.Integration.Events.StockControl.WriteOff;

/// <summary>
/// Интеграционное событие массового удаления списаний
/// </summary>
public class WriteOffBulkDeletedIntegrationEvent : BulkProductFlowIntegrationEvent
{
	public WriteOffBulkDeletedIntegrationEvent(IEnumerable<ProductFlowInfoDto> info, string creatorName)
		: base(info, creatorName)
	{
	}
}
