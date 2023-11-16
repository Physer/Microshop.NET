namespace Microshop.ContainerConfiguration;

public sealed class IndexContainerConfiguration : IContainerConfiguration
{
    public string ImageName => "getmeili/meilisearch";
    public int? Port => 7700;
    public string ApiKey => "integration_test_master_key";
    public int IntervalInSeconds = 3600;

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "MEILI_MASTER_KEY", ApiKey }
    };
}
