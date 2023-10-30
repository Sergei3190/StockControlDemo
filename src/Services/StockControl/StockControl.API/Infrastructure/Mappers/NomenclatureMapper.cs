using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Nomenclature;

namespace StockControl.API.Infrastructure.Mappers;

public static class NomenclatureMapper
{
	public static Nomenclature? CreateEntity(this NomenclatureDto dto, Guid userId) => dto is null
		? null
		: new Nomenclature()
		{
			Id = dto.Id,
			Name = dto.Name,
			ClassifierId = dto.Classifier.Id,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this Nomenclature entity, NomenclatureDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.Name = dto.Name;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static NomenclatureDto? CreateDto(this Nomenclature entity) => entity is null
		? null
		: new NomenclatureDto()
		{
			Id = entity.Id,
			Name = entity.Name,
			Classifier = entity.Classifier is not null ? new NamedEntityDto()
			{
				Id = entity.Classifier.Id,
				Name = entity.Classifier.Name,
			} : new NamedEntityDto()
		};

	public static NamedEntityDto? CreateSelectDto(this Nomenclature entity) => entity is null
		? null
		: new NamedEntityDto()
		{
			Id = entity.Id,
			Name = entity.Name
		};
}
