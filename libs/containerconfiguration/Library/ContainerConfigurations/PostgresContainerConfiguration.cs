namespace Microshop.ContainerConfiguration.ContainerConfigurations;

public sealed class PostgresContainerConfiguration : IContainerConfiguration
{
    public string ImageName => "postgres";

    public int? Port => 5432;

    public static string Username => "username";
    public static string Password => "password";
    public static string Database => "database";
    public string? ContainerIpAddress { get; set; }
    public int? PublicPort { get; set; }
    public string InternalConnectionString => $"postgresql://{Username}:{Password}@{ContainerIpAddress}:{Port}/{Database}";
    public string ExternalConnectionString => $"postgresql://{Username}:{Password}@localhost:{PublicPort}/{Database}";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "POSTGRES_USER", Username },
        { "POSTGRES_PASSWORD", Password },
        { "POSTGRES_DB", Database },
    };
}
