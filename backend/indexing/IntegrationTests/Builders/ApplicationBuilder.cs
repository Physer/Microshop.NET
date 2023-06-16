using Indexer;
using Microsoft.Extensions.Hosting;

namespace IntegrationTests.Builders;

internal class ApplicationBuilder
{
    private readonly IDictionary<string, string?> _configurationData;

    public ApplicationBuilder(IDictionary<string, string?> configurationData) => _configurationData = configurationData;

    public IHost Build() => Host
        .CreateDefaultBuilder()
        .ConfigureAppConfiguration(builder => builder.AddIntegrationAppConfiguration(_configurationData))
        .ConfigureServices(ServiceConfigurator.ConfigureServices)
        .Build();
}
