using PersonalCabinet.API.Domain.Person;
using PersonalCabinet.API.Models.DTO.UserPerson;

namespace PersonalCabinet.API.Infrastructure.Mappers;

public static class UserPersonMapper
{
	public static UserPersonDto? CreateDto(this UserPerson entity) => entity is null
		? null
		: new UserPersonDto()
		{
			Id = entity.Id,
			LastName = entity.LastName,
			FirstName = entity.FirstName,
			MiddleName = entity.MiddleName,
			Age = entity.Age,
			Birthday = entity.Birthday,
			CardId = entity.CardId
		};

	public static UserPerson? CreateEntity(this UserPersonDto dto, Guid? userId) => dto is null
		? null
		: new UserPerson()
		{
			Id = dto.Id,
			LastName = dto.LastName,
			FirstName = dto.FirstName,
			MiddleName = dto.MiddleName,
			Age = dto.Age,
			Birthday = dto.Birthday,
			CardId = dto.CardId,
			UserId = userId ?? throw new InvalidOperationException($"Пользователь не задан"),
			CreatedBy = userId,
			CreatedDate = DateTimeOffset.Now.ToLocalTime()
		};

	public static void UpdateEntity(this UserPerson entity, UserPersonDto dto, Guid? userId)
	{
		ArgumentNullException.ThrowIfNull(entity, nameof(entity));

		if (dto is null)
			return;

		entity.LastName = dto.LastName;
		entity.FirstName = dto.FirstName;
		entity.MiddleName = dto.MiddleName;
		entity.Age = dto.Age;
		entity.Birthday = dto.Birthday;

		entity.UpdatedBy = userId ?? throw new InvalidOperationException($"Пользователь не задан");
		entity.UpdatedDate = DateTimeOffset.Now.ToLocalTime();
	}
}
