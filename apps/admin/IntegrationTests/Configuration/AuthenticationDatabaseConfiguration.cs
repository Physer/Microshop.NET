using Microshop.ContainerConfiguration;

namespace IntegrationTests.Configuration;

internal class AuthenticationDatabaseConfiguration : IContainerConfiguration
{
    public string ImageName => "postgres";

    public int? Port => 5432;

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "POSTGRES_USER", Constants.DefaultTextValue },
        { "POSTGRES_PASSWORD", Constants.DefaultTextValue },
        { "POSTGRES_DB", Constants.DefaultTextValue }
    };
}
