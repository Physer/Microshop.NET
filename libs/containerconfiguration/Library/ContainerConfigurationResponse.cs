using DotNet.Testcontainers.Containers;
using Microshop.ContainerConfiguration.ContainerConfigurations;

namespace Microshop.ContainerConfiguration;

public class ContainerConfigurationResponse<T>(IContainer container, T configuration) where T : IContainerConfiguration
{
    public IContainer Container { get; init; } = container;
    public T Configuration { get; init; } = configuration;
}
