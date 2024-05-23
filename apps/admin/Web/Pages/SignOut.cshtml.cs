using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Services;

namespace Web.Pages;

public class SignOutModel(CookieService cookieService) : PageModel
{
    private readonly CookieService _cookieService = cookieService;

    public async Task<IActionResult> OnGetAsync()
    {
        await _cookieService.SignOutAsync();
        return Redirect("/");
    }
}
