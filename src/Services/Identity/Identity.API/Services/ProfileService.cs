using System.Security.Claims;

using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Models;

using Identity.API.Domain.Entities;

using IdentityModel;

using Microsoft.AspNetCore.Identity;

namespace Identity.API.Services;

public class ProfileService : ProfileService<User>
{
	private readonly UserManager<User> _userManager;
	private readonly ILogger<ProfileService> _logger;

	public ProfileService(
		UserManager<User> userManager,
		IUserClaimsPrincipalFactory<User> claimFactory,
		ILogger<ProfileService> logger) : base(userManager, claimFactory)
	{
		_userManager = userManager;
		_logger = logger;
	}

	/// <summary>
	/// This method is called whenever claims about the user are requested (e.g. during token creation or via the userinfo endpoint)
	/// </summary>
	/// <param name="context">The context.</param>
	/// <returns></returns>
	public override async Task GetProfileDataAsync(ProfileDataRequestContext context)
	{
		await base.GetProfileDataAsync(context);

		var user = await UserManager.GetUserAsync(context.Subject);

		// добавляем доп права
		context.IssuedClaims.AddRange(CollectClaims(user));

		// добавляем стандартные
		context.IssuedClaims.AddRange(await _userManager.GetClaimsAsync(user));
	}

	public static IEnumerable<Claim> CollectClaims(User user)
	{
		yield return new Claim(JwtClaimTypes.Name, user.UserName);
	}
}
