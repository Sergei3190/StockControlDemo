using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace Service.Common.Interfaces;

/// <summary>
/// CRUD интерфейс
/// </summary>
public interface ICrudService<TEntity, TFilter>
	where TEntity : EntityDto
	where TFilter : FilterDto
{
	Task<PaginatedItemsDto<TEntity>> GetListAsync(TFilter filter);
	Task<TEntity?> GetByIdAsync(Guid id);
	Task<Guid> CreateAsync(TEntity? dto);
	Task<bool> UpdateAsync(TEntity? dto);
	Task<bool> DeleteAsync(Guid id);
}