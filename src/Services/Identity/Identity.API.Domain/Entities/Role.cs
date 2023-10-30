using Microsoft.AspNetCore.Identity;

namespace Identity.API.Domain.Entities;

public class Role : IdentityRole<Guid>
{
	// роли по умолчанию 
	public const string Administrations = "Administrations";
	public const string Users = "Users";
}