
namespace Microshop.ContainerConfiguration;

internal sealed class SupertokensContainerConfiguration : IContainerConfiguration
{
    public string ImageName => "registry.supertokens.io/supertokens/supertokens-postgresql";

    public int? Port => 3567;

    public required string AuthenticationDatabaseConnectionString { get; init; }

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "POSTGRESQL_CONNECTION_URI", AuthenticationDatabaseConnectionString }
    };
}
