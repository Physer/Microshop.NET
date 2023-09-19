using Application;
using Application.DataGeneration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

[Authorize(Policy = Globals.Authorization.AdministratorPolicyName)]
public class GenerateDataModel : PageModel
{
    public bool? Success { get; set; } = null;

    private readonly IApiClient _apiClient;
    private readonly ILogger<GenerateDataModel> _logger;

    public GenerateDataModel(IApiClient apiClient,
        ILogger<GenerateDataModel> logger)
    {
        _apiClient = apiClient;
        _logger = logger;
    }

    public async Task OnPostAsync()
    {
        try
        {
            await _apiClient.GenerateProducts();
            Success = true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unable to generate data");
        }
    }
}
