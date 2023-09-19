using Application.DataManagement;
using DataManagement.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataManagement;

public static class DependencyRegistrator
{
    public static void RegisterDataManagementDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var dataGenerationOptionsSection = configuration.GetSection(DataManagementOptions.ConfigurationEntry);
        var dataGenerationOptions = dataGenerationOptionsSection.Get<DataManagementOptions>();
        if (dataGenerationOptions is null)
            throw new ArgumentNullException(nameof(dataGenerationOptions), "Invalid data management options");

        services.AddHttpClient<IApiClient, ApiClient>(configuration => configuration.BaseAddress = new Uri(dataGenerationOptions.BaseUrl));
    }
}