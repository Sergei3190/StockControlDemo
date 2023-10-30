using PersonalCabinet.API.Domain.Person;

using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.Infrastructure.Mappers;

public static class LoadedDataTypeMapper
{
	public static NamedEntityDto? CreateDto(this LoadedDataType entity) => entity is null
		? null
		: new NamedEntityDto()
		{
			Id = entity.Id,
			Name = entity.Name
		};
}
