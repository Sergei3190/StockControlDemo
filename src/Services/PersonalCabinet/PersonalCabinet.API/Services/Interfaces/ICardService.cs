using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.Services.Interfaces;

/// <summary>
/// Сервис получения объекта по карте пользователя 
/// </summary>
public interface ICardService<TEntity>
	where TEntity : EntityDto
{
	Task<TEntity?> GetByCardIdAsync(Guid cardId);
}