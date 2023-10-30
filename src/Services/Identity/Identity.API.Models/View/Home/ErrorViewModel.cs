using Duende.IdentityServer.Models;

namespace Identity.API.Models.View.Home;

public class ErrorViewModel
{
	public ErrorViewModel()
	{
	}

	public ErrorViewModel(string error)
	{
		Error = new ErrorMessage { Error = error };
	}

	public ErrorMessage Error { get; set; }
}
