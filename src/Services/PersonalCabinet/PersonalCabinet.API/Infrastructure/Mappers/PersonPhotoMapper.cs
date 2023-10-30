using PersonalCabinet.API.Domain.Person;
using PersonalCabinet.API.Models.DTO.Photo;

using Service.Common.DTO;
using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.Infrastructure.Mappers;

public static class PersonPhotoMapper
{
	public static PhotoDto? CreateDto(this PersonPhoto entity) => entity is null
		? null
		: new PhotoDto()
		{
			Id = entity.Id,
			CardId = entity.CardId,
			// в случаи, если данных в кэше нет, то возвращаем объект только с id, чтобы клиент мог понять, что данных нет и запросил бы напрямую из 
			// хранилища файлов
			File = new FileDto() { Id = entity.ExternalId },
			LoadedDataType = new NamedEntityDto()
			{
				Id = entity.LoadedDataType.Id,
				Name = entity.LoadedDataType.Name,
			}
		};

	public static PersonPhoto? CreateEntity(this PhotoDto dto, Guid userId) => dto is null
		? null
		: new PersonPhoto()
		{
			Id = dto.Id,
			CardId = dto.CardId,
			ExternalId = dto.File.Id,
			FileName = dto.File.FileName,
			LoadedDataTypeId = dto.LoadedDataType!.Id,
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this PersonPhoto entity, PhotoDto dto, Guid userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.ExternalId = dto.File.Id;
		entity.FileName = dto.File.FileName;

		entity.UpdatedBy = userId;
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}
}
