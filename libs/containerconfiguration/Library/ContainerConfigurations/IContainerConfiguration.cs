namespace Microshop.ContainerConfiguration.ContainerConfigurations;

public interface IContainerConfiguration
{
    public string ImageName { get; }
    public int? Port { get; }
    public IReadOnlyDictionary<string, string>? EnvironmentVariables { get; }
}
