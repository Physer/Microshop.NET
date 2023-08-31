using Application.Authentication;
using Application.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Web.Pages;

[AllowAnonymous]
[BindProperties]
public class SignInModel : PageModel
{
    public string? Username { get; set; }
    public string? Password { get; set; }

    private readonly IAuthenticationClient _authenticationClient;
    private readonly ILogger<SignInModel> _logger;

    public SignInModel(IAuthenticationClient authenticationClient,
        ILogger<SignInModel> logger)
    {
        _authenticationClient = authenticationClient;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var returnUrl = "/";
        if (RouteData.Values.TryGetValue("ReturnUrl", out var existingReturnUrlValue) && existingReturnUrlValue is string existingReturnUrl && !string.IsNullOrWhiteSpace(existingReturnUrl))
            returnUrl = existingReturnUrl;

        try
        {
            _logger.LogInformation("Attempting login attempt for user: {username}", Username);
            var authenticationResult = await _authenticationClient.SignInAsync(Username!, Password!);
            List<Claim> claims = new()
            {
                new(ClaimTypes.Name, Username!)
            };
            claims.AddRange(authenticationResult.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            _logger.LogInformation("Succesfully logged in user: {username}", Username);
        }
        catch (AuthenticationException authenticationException)
        {
            _logger.LogWarning(authenticationException, "Invalid login attempt or invalid credentials for user: {username}", Username);
            ModelState.AddModelError(string.Empty, "Invalid credentials or permissions");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failure logging in for user: {username}", Username);
            ModelState.AddModelError(string.Empty, "Something went wrong, please try again later");
        }

        return Redirect(returnUrl);
    }
}