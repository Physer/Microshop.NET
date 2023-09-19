using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

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
