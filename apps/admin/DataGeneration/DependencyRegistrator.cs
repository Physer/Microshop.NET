using Application.DataGeneration;
using DataGeneration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataGeneration;

public static class DependencyRegistrator
{
    public static void RegisterDataGenerationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var dataGenerationOptionsSection = configuration.GetSection(DataGenerationOptions.ConfigurationEntry);
        var dataGenerationOptions = dataGenerationOptionsSection.Get<DataGenerationOptions>();
        if (dataGenerationOptions is null)
            throw new ArgumentNullException(nameof(dataGenerationOptions), "Invalid data generation options");

        services.AddHttpClient<IApiClient, ApiClient>(configuration => configuration.BaseAddress = new Uri(dataGenerationOptions.BaseUrl));
    }
}