using Microsoft.AspNetCore.Http;

namespace Application;

public static class Globals
{
    public static class Authorization
    {
        public const string AdministratorPolicyName = "RequiresAdmin";
        public const string AdministratorRole = "admin";
    }

    public static class Cookies
    {
        public const string AuthorizationTokenCookieName = "msJwt";
        public static readonly CookieOptions DefaultOptions = new()
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true
        };
    }

    public static class Http
    {
        public const string BearerAuthenticationScheme = "Bearer";
    }
}
