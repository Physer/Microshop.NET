using Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

[Authorize(Policy = Globals.Authorization.AdministratorPolicyName)]
public class IndexModel : PageModel
{
    public void OnGet() { }
}