using Application;
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
public class SignInModel(IAuthenticationClient authenticationClient,
    ILogger<SignInModel> logger) : PageModel
{
    public required string Username { get; set; }
    public required string Password { get; set; }

    private readonly IAuthenticationClient _authenticationClient = authenticationClient;
    private readonly ILogger<SignInModel> _logger = logger;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            _logger.LogInformation("Attempting sign in for user: {username}", Username);
            var authenticationResult = await _authenticationClient.SignInAsync(Username, Password);
            List<Claim> claims =
            [
                new(ClaimTypes.Name, Username!),
                .. authenticationResult.Roles.Select(role => new Claim(ClaimTypes.Role, role)),
            ];
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            _logger.LogInformation("Setting authorization token in cookie");
            HttpContext.Response.Cookies.Append(Globals.Cookies.AuthorizationTokenCookieName, authenticationResult.AccessToken, Globals.Cookies.DefaultOptions);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            _logger.LogInformation("Succesfully signed in for user: {username}", Username);
        }
        catch (AuthenticationException authenticationException)
        {
            _logger.LogWarning(authenticationException, "Invalid sign in attempt or invalid credentials for user: {username}", Username);
            ModelState.AddModelError(string.Empty, "Invalid credentials or permissions");
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failure signin in for user: {username}", Username);
            ModelState.AddModelError(string.Empty, "Something went wrong, please try again later");
        }

        if (ModelState.ErrorCount > 0)
            return Page();

        return Redirect("/");
    }
}