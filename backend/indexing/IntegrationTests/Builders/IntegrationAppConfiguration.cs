using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Builders;


internal static class IntegrationAppConfigurationExtensions
{
    public static IConfigurationBuilder AddIntegrationAppConfiguration(this IConfigurationBuilder builder, IDictionary<string, string?> configurationData)
        => builder.Add(new IntegrationAppConfigurationSource(configurationData));
}

internal class IntegrationAppConfigurationSource : IConfigurationSource
{
    private readonly IDictionary<string, string?> _configurationData;

    public IntegrationAppConfigurationSource(IDictionary<string, string?> configurationData) => _configurationData = configurationData;

    public IConfigurationProvider Build(IConfigurationBuilder builder) => new IntegrationAppConfigurationProvider(_configurationData);
}

internal class IntegrationAppConfigurationProvider : ConfigurationProvider
{
    private readonly IDictionary<string, string?> _configurationData;

    public IntegrationAppConfigurationProvider(IDictionary<string, string?> configurationData) => _configurationData = configurationData;

    public override void Load() => Data = _configurationData;
}


