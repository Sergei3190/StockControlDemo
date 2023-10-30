using Service.Common.DTO.Entities.Base;

using StockControl.API.Domain.Stock;
using StockControl.API.Models.DTO.Party;

namespace StockControl.API.Infrastructure.Mappers;

public static class PartyMapper
{
	public static Party? CreateEntity(this PartyDto dto, Guid userId) => dto is null
		? null
		: new Party()
		{
			Id = dto.Id,
			Number = dto.Number,
			CreateDate = dto.CreateDate,
			ExtensionNumber = dto.ExtensionNumber,
			CreateTime = dto.CreateTime,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this Party entity, PartyDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.Number = dto.Number;
		entity.CreateDate = dto.CreateDate;
		entity.CreateTime = dto.CreateTime;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}

	public static PartyDto? CreateDto(this Party entity) => entity is null
		? null
		: new PartyDto()
		{
			Id = entity.Id,
			Number = entity.Number,
			ExtensionNumber = entity.ExtensionNumber,
			CreateDate = entity.CreateDate,
			CreateTime = entity.CreateTime,
		};

	public static NamedEntityDto? CreateSelectDto(this Party entity) => entity is null
		? null
		: new NamedEntityDto()
		{
			Id = entity.Id,
			Name = $"№ {entity.Number} / {entity.ExtensionNumber} от {entity.CreateDate} {entity.CreateTime}"
		};
}
