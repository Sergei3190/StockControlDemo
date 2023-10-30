using Service.Common.DTO.Entities.Base;

namespace PersonalCabinet.API.Models.DTO.UserPerson;

/// <summary>
/// Персона пользователя
/// </summary>
public class UserPersonDto : EntityDto
{
	public string LastName { get; set; }
	public string FirstName { get; set; }
	public string? MiddleName { get; set; }
	public int? Age { get; set; }
	public DateOnly? Birthday { get; set; }
	public Guid CardId { get; set; }
	public Guid? UserId { get; set; }

	public override string ToString() => $"({nameof(Id)}: {Id}):" +
		$"{nameof(LastName)}: {LastName}" +
		$"{nameof(FirstName)}: {FirstName}" +
		$"{nameof(MiddleName)}: {MiddleName}" +
		$"{nameof(Age)}: {Age}" +
		$"{nameof(Birthday)}: {Birthday}" +
		$"{nameof(CardId)}: {CardId}";
}