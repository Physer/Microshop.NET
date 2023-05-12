namespace IntegrationTests.Configuration;

internal class MicroshopProductsContainerConfiguration : IContainerConfiguration
{
    private readonly string _rabbitMqContainerName;
    private readonly string _rabbitMqUsername;
    private readonly string _rabbitMqPassword;

    public MicroshopProductsContainerConfiguration(string rabbitMqContainerName, string rabbitMqUsername, string rabbitMqPassword)
    {
        _rabbitMqContainerName = rabbitMqContainerName;
        _rabbitMqUsername = rabbitMqUsername;
        _rabbitMqPassword = rabbitMqPassword;
    }

    public string ImageName => "physer/microshop-products";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "Servicebus__BaseUrl", _rabbitMqContainerName },
        { "Servicebus__ManagementUsername", _rabbitMqUsername },
        { "Servicebus__ManagementPassword", _rabbitMqPassword },
    };
}
