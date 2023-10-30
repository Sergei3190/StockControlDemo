using Service.Common.DTO;

namespace Service.Common.Interfaces;

/// <summary>
/// Сервис массового удаления
/// </summary>
public interface IBulkDeleteService
{
	Task<BulkDeleteResultDto?> BulkDeleteAsync(params Guid[] ids);
}
