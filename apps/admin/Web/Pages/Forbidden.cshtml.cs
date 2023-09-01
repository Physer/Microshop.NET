using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

[AllowAnonymous]
public class ForbiddenModel : PageModel
{
    public IActionResult OnPost() => Redirect("/SignOut");
}