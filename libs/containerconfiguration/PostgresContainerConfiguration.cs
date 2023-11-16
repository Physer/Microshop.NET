namespace Microshop.ContainerConfiguration;

public sealed class PostgresContainerConfiguration : IContainerConfiguration
{
    public string ImageName => "postgres";

    public int? Port => 5432;

    public static string Username => "username";
    public static string Password => "password";
    public static string Database => "database";
    public static string ConnectionString => $"postgresql://{Username}:{Password}@localhost:5432/{Database}";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "POSTGRES_USER", Username },
        { "POSTGRES_PASSWORD", Password },
        { "POSTGRES_DB", Database },
    };
}
