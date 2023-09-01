using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Services;

namespace Web.Pages;

public class SignOutModel : PageModel
{
    private readonly CookieService _cookieService;

    public SignOutModel(CookieService cookieService) => _cookieService = cookieService;

    public async Task<IActionResult> OnGetAsync()
    {
        await _cookieService.SignOutAsync();
        return Redirect("/");
    }
}
