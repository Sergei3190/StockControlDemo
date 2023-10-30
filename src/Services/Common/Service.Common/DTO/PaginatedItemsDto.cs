namespace Service.Common.DTO;

/// <summary>
/// Элементы с разбивкой по страницам
/// </summary>
public class PaginatedItemsDto<TEntity> where TEntity : class
{
	public int Page { get; private set; }
	public int PageSize { get; private set; }
	public int TotalPages { get; private set; }
	public int TotalItems { get; private set; }
	public IEnumerable<TEntity> Items { get; private set; }

	public PaginatedItemsDto()
		: this(0, 20, 0, new List<TEntity>())
	{

	}

	public PaginatedItemsDto(int page, int pageSize, int totalItems, IEnumerable<TEntity> items)
	{
		Page = page <= 0 ? 1 : page;
		PageSize = pageSize < 0 ? 20 : pageSize;
		TotalItems = totalItems;
		TotalPages = (int)Math.Ceiling((double)TotalItems / PageSize);
		Items = items;
	}
}
