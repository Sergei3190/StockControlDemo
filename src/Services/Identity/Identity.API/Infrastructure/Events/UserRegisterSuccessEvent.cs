using Duende.IdentityServer.Events;

namespace Identity.API.Infrastructure.Events;

public class UserRegisterSuccessEvent : Event
{
	public UserRegisterSuccessEvent(string username, string clientId = null, string roles = null)
		: base(
			"Authorization",
			"User Register Success",
			EventTypes.Success,
			EventIds.DeviceAuthorizationFailure)
	{
		Username = username;
		ClientId = clientId;
		Roles = roles;
	}

	public string Username { get; set; }

	public string ClientId { get; set; }

	public string Roles { get; set; }
}
