using Microsoft.AspNetCore.Http;

namespace Application.Authentication;

internal class TokenRetriever : ITokenRetriever
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenRetriever(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public string GetAccessTokenFromCookie()
    {
        if (!_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue(Globals.Cookies.AuthorizationTokenCookieName, out var accessToken) || string.IsNullOrWhiteSpace(accessToken))
            throw new UnauthorizedAccessException("No JWT present in cookie");

        return accessToken;
    }
}
