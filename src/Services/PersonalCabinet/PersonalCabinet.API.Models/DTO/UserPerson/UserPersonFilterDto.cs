using Service.Common.DTO;

namespace PersonalCabinet.API.Models.DTO.UserPerson;

/// <summary>
/// Фильтр персон пользователей
/// </summary>
public class UserPersonFilterDto : FilterDto
{
	public int? Age { get; set; }
	public DateOnly? Birthday { get; set; }
}