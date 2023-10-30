using System.Text.Json;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Service.Common.Binders;

public class JsonQueryBinder : IModelBinder
{
	private readonly ILogger<JsonQueryBinder> _logger;

	public JsonQueryBinder(ILogger<JsonQueryBinder> logger)
	{
		_logger = logger;
	}

	public Task BindModelAsync(ModelBindingContext bindingContext)
	{
		var value = bindingContext.ValueProvider.GetValue(bindingContext.FieldName).FirstValue ?? GetValueFromQuery(bindingContext.HttpContext.Request.Query);

		if (value == null)
		{
			return Task.CompletedTask;
		}

		try
		{
			var parsed = JsonSerializer.Deserialize(
				value,
				bindingContext.ModelType,
				new JsonSerializerOptions(JsonSerializerDefaults.Web)
			);
			bindingContext.Result = ModelBindingResult.Success(parsed);
		}
		catch (Exception e)
		{
			_logger.LogError(e, "Ошибка привязки строки запроса к модели '{FieldName}'", bindingContext.FieldName);
			bindingContext.Result = ModelBindingResult.Failed();
		}

		return Task.CompletedTask;
	}

	// в случаи, если обращение идёт не из spa а из просто api, например из swaggerApi, то нам надо уметь читать данные из строки запроса
	private string? GetValueFromQuery(IQueryCollection query)
	{
		var result = new Dictionary<string, string>();

		query.ToList().ForEach(d =>
		{
			result.Add(d.Key, d.Value.FirstOrDefault()!);
		});

		return JsonSerializer.Serialize(result);
	}
}