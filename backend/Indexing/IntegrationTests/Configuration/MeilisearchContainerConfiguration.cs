namespace IntegrationTests.Configuration;

internal class MeilisearchContainerConfiguration : IContainerConfiguration
{
    public string ImageName => "getmeili/meilisearch";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "MEILI_MASTER_KEY", "integration_test_master_key" }
    };
}
