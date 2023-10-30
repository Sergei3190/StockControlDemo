using Duende.IdentityServer.Events;

namespace Identity.API.Infrastructure.Events;

public class UserRegisterFailureEvent : Event
{
	public UserRegisterFailureEvent(string username, string error, string clientId = null)
		: base(
			"Authorization",
			"User Register Failure",
			EventTypes.Failure,
			EventIds.DeviceAuthorizationFailure,
			error)
	{
		Username = username;
		ClientId = clientId;
	}

	public string Username { get; set; }

	public string ClientId { get; set; }
}
