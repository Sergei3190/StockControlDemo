using System.Text.Json.Serialization;

namespace Service.Common.DTO;

/// <summary>
/// Базовая фильтрация коллекции возвращаемых объектов, данный фильтр должен передаваться в строке параметров запроса
/// </summary>
public abstract class FilterDto
{
	private int _page;
	private int _pageSize;

	/// <summary>
	/// Строка поиска в интерфейсе пользователя
	/// </summary>
	public string? Search { get; set; }

	public int Page 
	{ 
		get => _page; 
		set => _page = value <= 0 ? 1 : value;
	}

	public int PageSize 
	{
		get => _pageSize;
		set => _pageSize = value <= 0 ? 20 : value;
	}

	[JsonIgnore]
	public int Skip => (Page - 1) * PageSize;

	[JsonIgnore]
	public int Take => PageSize;

	public FilterDto()
		: this(null, 1, 20)
	{

	}

	public FilterDto(string? search, int page, int pageSize)
	{
		Search = search;
		Page = page;
		PageSize = pageSize;
	}
}
