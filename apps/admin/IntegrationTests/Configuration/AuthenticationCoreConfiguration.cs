using Microshop.ContainerConfiguration;

namespace IntegrationTests.Configuration;

internal class AuthenticationCoreConfiguration : IContainerConfiguration
{
    public string ImageName => "registry.supertokens.io/supertokens/supertokens-postgresql";

    public int? Port => 3567;

    public required string AuthenticationDatabaseIpAddress { get; init; }
    public required int AuthenticationDatabasePort { get; init; }

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "POSTGRESQL_CONNECTION_URI", $"postgresql://{Constants.DefaultTextValue}:{Constants.DefaultTextValue}@{AuthenticationDatabaseIpAddress}:{AuthenticationDatabasePort}/{Constants.DefaultTextValue}" }
    };
}
