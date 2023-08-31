using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Services;

namespace Web.Pages;

[AllowAnonymous]
public class ForbiddenModel : PageModel
{
    private readonly CookieService _cookieService;

    public ForbiddenModel(CookieService cookieService) => _cookieService = cookieService;

    public async Task<IActionResult> OnPostAsync()
    {
        await _cookieService.SignOutAsync();
        return Redirect("/");
    }
}