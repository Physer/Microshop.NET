namespace Web.Utilities;

public static class Globals
{
    public static class Authorization
    {
        public const string AdministratorPolicyName = "RequiresAdmin";
        public const string AdministratorRole = "admin";
        public const string AuthorizationTokenCookieName = "msJwt";
    }

    public static class Cookies
    {
        public static readonly CookieOptions DefaultOptions = new()
        {
            SameSite = SameSiteMode.Strict,
            HttpOnly = true
        };
    }
}
