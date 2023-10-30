using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Service.Common.DelegatingHandlers;

/// <summary>
/// Устанавливает токен доступа в заголовок Bearer для проверки системой Identity
/// </summary>
public class HttpClientAuthorizationDelegatingHandler
    : DelegatingHandler
{
	// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-7.0
	private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
    {
        if (_httpContextAccessor.HttpContext is HttpContext context)
        {
            var accessToken = await context.GetTokenAsync("access_token").ConfigureAwait(false);

            if (accessToken is not null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
