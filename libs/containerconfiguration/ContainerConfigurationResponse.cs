using DotNet.Testcontainers.Containers;

namespace Microshop.ContainerConfiguration;

public class ContainerConfigurationResponse<T> where T : IContainerConfiguration
{
    public ContainerConfigurationResponse(IContainer container,
        T configuration)
    {
        Container = container;
        Configuration = configuration;
    }

    public IContainer Container { get; init; }
    public T Configuration { get; init; }
}
