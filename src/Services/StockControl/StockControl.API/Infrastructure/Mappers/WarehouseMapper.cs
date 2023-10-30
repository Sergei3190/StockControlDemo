using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Warehouse;

namespace StockControl.API.Infrastructure.Mappers;

public static class WarehouseMapper
{
	public static Warehouse? CreateEntity(this WarehouseDto dto, Guid userId) => dto is null
		? null
		: new Warehouse()
		{
			Id = dto.Id,
			Name = dto.Name,
			ClassifierId = dto.Classifier.Id,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this Warehouse entity, WarehouseDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.Name = dto.Name;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static WarehouseDto? CreateDto(this Warehouse entity) => entity is null
		? null
		: new WarehouseDto()
		{
			Id = entity.Id,
			Name = entity.Name,
			Classifier = entity.Classifier is not null ? new NamedEntityDto()
			{
				Id = entity.Classifier.Id,
				Name = entity.Classifier.Name,
			} : new NamedEntityDto()
		};

	public static NamedEntityDto? CreateSelectDto(this Warehouse entity) => entity is null
		? null
		: new NamedEntityDto()
		{
			Id = entity.Id,
			Name = entity.Name
		};
}
