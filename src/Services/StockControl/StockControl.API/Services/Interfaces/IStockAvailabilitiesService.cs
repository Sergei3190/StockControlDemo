using Service.Common.Interfaces;

using StockControl.API.Models.DTO.StockAvailability;

namespace StockControl.API.Services.Interfaces;

/// <summary>
/// Сервис остатков
/// </summary>
public interface IStockAvailabilitiesService : ICrudService<StockAvailabilityDto, StockAvailabilityFilterDto>, IBulkDeleteService
{
	/// <summary>
	/// Получить отстаток на складе отправителе
	/// </summary>
	Task<StockAvailabilityDto?> GetRemainderOfSenderWarehouseAsync(Guid sendingWarehouseId, Guid partyId);
}