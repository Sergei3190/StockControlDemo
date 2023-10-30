using PersonalCabinet.API.Models.DTO.UserPerson;

using Service.Common.Interfaces;

namespace PersonalCabinet.API.Services.Interfaces;

/// <summary>
/// Сервис персон пользователя
/// </summary>
public interface IUserPersonsService : ICrudService<UserPersonDto, UserPersonFilterDto>, ICardService<UserPersonDto>
{
	// тк пользователя берём из контекста запроса и на фронте не храним в dto userId, то ничего не принимаем на вход
	Task<UserPersonDto?> GetPersonCurrentUserAsync();
}