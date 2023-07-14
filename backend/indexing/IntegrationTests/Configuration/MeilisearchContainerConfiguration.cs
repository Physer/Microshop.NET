namespace IntegrationTests.Configuration;

internal class MeilisearchContainerConfiguration : IContainerConfiguration
{
    public string ImageName => "getmeili/meilisearch";
    public static int Port => 7700;
    public static string ApiKey => "integration_test_master_key";
    public static int IntervalInSeconds = 3600;

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "MEILI_MASTER_KEY", ApiKey }
    };
}
