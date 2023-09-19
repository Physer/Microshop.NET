using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Utilities;

namespace Web.Pages;

[Authorize(Policy = AuthorizationDefaults.AdministratorPolicyName)]
public class GenerateDataModel : PageModel
{
    public bool GeneratedData { get; set; }
    public bool Success { get; set; }

    public async Task OnPostAsync()
    {
        GeneratedData = true;
        Success = true;
    }
}
