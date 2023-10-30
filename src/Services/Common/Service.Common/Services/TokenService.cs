using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Http;

using Service.Common.Interfaces;

namespace Service.Common.Services;

public class TokenService : ITokenService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public TokenService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
	}

	public async Task<string> GetTokenAsync(CancellationToken cancellationToken = default)
	{
		string? accessToken = null!;

		if (_httpContextAccessor.HttpContext is HttpContext context)
			accessToken = await context.GetTokenAsync("access_token").ConfigureAwait(false);

		return accessToken!;
	}
}
