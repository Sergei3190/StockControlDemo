using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;

namespace StockControl.API.Infrastructure.Mappers;

public static class ProductFlowTypeMapper
{
	public static NamedEntityDto? CreateDto(this ProductFlowType entity) => entity is null
		? null
		: new NamedEntityDto()
		{
			Id = entity.Id,
			Name = entity.Name
		};
}
