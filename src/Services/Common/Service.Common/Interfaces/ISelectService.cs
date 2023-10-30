using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace Service.Common.Interfaces;

/// <summary>
/// Интерфейс получения статичных данных БД
/// </summary>
public interface ISelectService<TEntity, TFilter>
	where TEntity : EntityDto
	where TFilter : FilterDto
{
	Task<PaginatedItemsDto<TEntity>> SelectAsync(TFilter filter);
}