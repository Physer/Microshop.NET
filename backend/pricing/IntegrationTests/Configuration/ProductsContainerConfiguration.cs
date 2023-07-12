namespace IntegrationTests.Configuration;

internal class ProductsContainerConfiguration : IContainerConfiguration
{
    private readonly string _rabbitMqContainerName;
    private readonly string _rabbitMqUsername;
    private readonly string _rabbitMqPassword;
    private readonly int _rabbitMqPort;

    public ProductsContainerConfiguration(string rabbitMqContainerName, 
        string rabbitMqUsername, 
        string rabbitMqPassword,
        int rabbitMqPort)
    {
        _rabbitMqContainerName = rabbitMqContainerName;
        _rabbitMqUsername = rabbitMqUsername;
        _rabbitMqPassword = rabbitMqPassword;
        _rabbitMqPort = rabbitMqPort;
    }

    public static int Port => 80;

    public string ImageName => "physer/microshop-products:main";

    public IReadOnlyDictionary<string, string>? EnvironmentVariables => new Dictionary<string, string>
    {
        { "Servicebus__BaseUrl", _rabbitMqContainerName },
        { "Servicebus__ManagementUsername", _rabbitMqUsername },
        { "Servicebus__ManagementPassword", _rabbitMqPassword },
        { "Servicebus__Port", _rabbitMqPort.ToString() }
    };
}
