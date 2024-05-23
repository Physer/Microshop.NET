using Application;
using Application.DataManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages;

[Authorize(Policy = Globals.Authorization.AdministratorPolicyName)]
public class ManageDataModel(IApiClient apiClient,
    ILogger<ManageDataModel> logger) : PageModel
{
    public bool? Success { get; set; } = null;

    private readonly IApiClient _apiClient = apiClient;
    private readonly ILogger<ManageDataModel> _logger = logger;

    public async Task OnPostGenerateAsync()
    {
        try
        {
            _ = await _apiClient.GenerateProductsAsync();
            Success = true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unable to generate data");
            Success = false;
        }
    }

    public async Task OnPostClearAsync()
    {
        try
        {
            _ = await _apiClient.ClearDataAsync();
            Success = true;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Unable to clear data");
            Success = false;
        }
    }
}
