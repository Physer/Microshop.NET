namespace IntegrationTests.Configuration;

internal class MeilisearchContainerConfiguration : IContainerConfiguration
{
    public string ImageName => "getmeili/meilisearch";
    public int Port => 7700;

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "MEILI_MASTER_KEY", "integration_test_master_key" }
    };
}
