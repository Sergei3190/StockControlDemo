using Microsoft.AspNetCore.Mvc;

using Service.Common.Binders;

namespace Service.Common.Attributes;

/// <summary>
/// Атрибут, который помогает парсить параметры строки запроса в сложные объекты
/// </summary>
public class FromJsonQueryAttribute : ModelBinderAttribute
{
	public FromJsonQueryAttribute()
	{
		BinderType = typeof(JsonQueryBinder);
	}
}
