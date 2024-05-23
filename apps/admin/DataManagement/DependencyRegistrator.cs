using Application.DataManagement;
using DataManagement.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("UnitTests")]
namespace DataManagement;

public static class DependencyRegistrator
{
    public static void RegisterDataManagementDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var dataGenerationOptionsSection = configuration.GetSection(DataManagementOptions.ConfigurationEntry);
        var dataGenerationOptions = dataGenerationOptionsSection.Get<DataManagementOptions>();
        ArgumentNullException.ThrowIfNull(dataGenerationOptions, nameof(dataGenerationOptions));

        services.AddHttpClient<IApiClient, ApiClient>(configuration => configuration.BaseAddress = new Uri(dataGenerationOptions.BaseUrl));
    }
}