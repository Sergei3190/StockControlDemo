using Service.Common.Integration.DTO;

namespace Service.Common.Integration.Events.StockControl.Moving;

/// <summary>
/// Интеграционное событие массового удаления перемещения
/// </summary>
public class MovingBulkDeletedIntegrationEvent : BulkProductFlowIntegrationEvent
{
	public MovingBulkDeletedIntegrationEvent(IEnumerable<ProductFlowInfoDto> info, string creatorName)
		: base(info, creatorName)
	{
	}
}
