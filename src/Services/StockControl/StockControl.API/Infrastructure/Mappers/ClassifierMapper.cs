using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Classifier;

namespace StockControl.API.Infrastructure.Mappers;

public static class ClassifierMapper
{
	public static ClassifierDto? CreateDto(this Classifier entity) => entity is null
		? null
		: new ClassifierDto()
		{
			Id = entity.Id,
			Name = entity.Name,
			Path = entity.Mnemo?.ToLower()
		};
}
