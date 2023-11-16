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

    protected IContainer Container { get; init; }
    protected T Configuration { get; init; }
}
