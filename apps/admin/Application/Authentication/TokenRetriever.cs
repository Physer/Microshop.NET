using Microsoft.AspNetCore.Http;

namespace Application.Authentication;

internal class TokenRetriever(IHttpContextAccessor httpContextAccessor) : ITokenRetriever
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetAccessTokenFromCookie()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null || !httpContext.Request.Cookies.TryGetValue(Globals.Cookies.AuthorizationTokenCookieName, out var accessToken) || string.IsNullOrWhiteSpace(accessToken))
            throw new UnauthorizedAccessException("No JWT present in cookie");

        return accessToken;
    }
}
