using Microsoft.AspNetCore.Identity;

namespace Identity.API.Domain.Entities;

public class User : IdentityUser<Guid>
{
	// пользователь по умолчанию 
	public const string Administrator = "Admin";
	public const string AdminPassword = "admin_123";

	public override string ToString() => UserName!;
}