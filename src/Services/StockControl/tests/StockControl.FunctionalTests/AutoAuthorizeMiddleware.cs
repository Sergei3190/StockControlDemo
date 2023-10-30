namespace StockControl.FunctionalTests;

/// <summary>
/// Промежуточное ПО для имитации получения авторизационного запроса клиента
/// </summary>
class AutoAuthorizeMiddleware
{
	public const string IDENTITY_ID = "D9A8F3AE-9F89-4B32-BEA2-19D9F10DF8A3";

	private readonly RequestDelegate _next;

	public AutoAuthorizeMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext httpContext)
	{
		var identity = new ClaimsIdentity("cookies");

		identity.AddClaim(new Claim("sub", IDENTITY_ID));
		identity.AddClaim(new Claim("unique_name", IDENTITY_ID));
		identity.AddClaim(new Claim(ClaimTypes.Name, IDENTITY_ID));

		httpContext.User.AddIdentity(identity);

		await _next.Invoke(httpContext);
	}
}
