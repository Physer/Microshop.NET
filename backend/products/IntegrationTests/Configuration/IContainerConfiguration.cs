namespace IntegrationTests.Configuration;

internal interface IContainerConfiguration
{
    public string ImageName { get; }
    public IReadOnlyDictionary<string, string>? EnvironmentVariables { get; }
}
