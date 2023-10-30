using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Organization;

namespace StockControl.API.Infrastructure.Mappers;

public static class OrganizationMapper
{
	public static Organization? CreateEntity(this OrganizationDto dto, Guid userId) => dto is null
		? null
		: new Organization()
		{
			Id = dto.Id,
			Name = dto.Name,
			ClassifierId = dto.Classifier.Id,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this Organization entity, OrganizationDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.Name = dto.Name;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static OrganizationDto? CreateDto(this Organization entity) => entity is null
		? null
		: new OrganizationDto()
		{
			Id = entity.Id,
			Name = entity.Name,
			Classifier = entity.Classifier is not null ? new NamedEntityDto()
			{
				Id = entity.Classifier.Id,
				Name = entity.Classifier.Name,
			} : new NamedEntityDto()
		};

	public static NamedEntityDto? CreateSelectDto(this Organization entity) => entity is null
		? null
		: new NamedEntityDto()
		{
			Id = entity.Id,
			Name = entity.Name
		};
}
