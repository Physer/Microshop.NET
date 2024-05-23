using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Web.Services;

public class CookieService(IHttpContextAccessor httpContextAccessor,
    ILogger<CookieService> logger)
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<CookieService> _logger = logger;

    public async Task SignOutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            throw new ArgumentNullException(nameof(httpContext), "Invalid HTTP context");
        var username = httpContext.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Name))?.Value;
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentNullException(nameof(username), "Unable to retrieve the current user's username from the HTTP context");

        try
        {
            _logger.LogInformation("Attempting to sign out for user: {username}", username);
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("Succesfully signed out for user: {username}", username);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unable to sign out for user: {username}", httpContext.User?.Claims?.FirstOrDefault(claim => claim.Type.Equals(ClaimTypes.Name))?.Value);
            throw;
        }
    }
}
