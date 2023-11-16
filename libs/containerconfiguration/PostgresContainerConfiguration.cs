namespace Microshop.ContainerConfiguration;

public sealed class PostgresContainerConfiguration : IContainerConfiguration
{
    public string ImageName => "postgres";

    public int? Port => 5432;

    public string Username => "username";
    public string Password => "password";
    public string Database => "database";
    public string ConnectionString => $"postgresql://{Username}:{Password}@localhost:5432/{Database}";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "POSTGRES_USER", Username },
        { "POSTGRES_PASSWORD", Password },
        { "POSTGRES_DB", Database },
    };
}
