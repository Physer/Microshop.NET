using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Microshop.ContainerConfiguration;

public static class ContainerFactory
{
    public static async Task<IContainer> InitializeRabbitMqContainer()
    {
        var rabbitMqConfiguration = new RabbitMqContainerConfiguration();
        var rabbitMqContainer = new ContainerBuilder()
            .WithImage(rabbitMqConfiguration.ImageName)
            .WithEnvironment(rabbitMqConfiguration.EnvironmentVariables)
            .WithPortBinding(rabbitMqConfiguration.Port!.Value, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilMessageIsLogged(@"[s|S]erver startup complete")
                .UntilPortIsAvailable(rabbitMqConfiguration.Port!.Value))
            .Build();
        await rabbitMqContainer.StartAsync().ConfigureAwait(false);
        return rabbitMqContainer;
    }

    public static async Task<IContainer> InitializeMeilisearchContainer()
    {
        var meilisearchConfiguration = new MeilisearchContainerConfiguration();
        var meilisearchContainer = new ContainerBuilder()
            .WithImage(meilisearchConfiguration.ImageName)
            .WithEnvironment(meilisearchConfiguration.EnvironmentVariables)
            .WithPortBinding(meilisearchConfiguration.Port!.Value, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(meilisearchConfiguration.Port!.Value))
            .Build();
        await meilisearchContainer.StartAsync().ConfigureAwait(false);
        return meilisearchContainer;
    }

    public static async Task<IContainer> InitializeCustomContainer(IContainerConfiguration customContainerConfiguration)
    {
        var containerBuilder = new ContainerBuilder()
            .WithImage(customContainerConfiguration.ImageName)
            .WithEnvironment(customContainerConfiguration.EnvironmentVariables);

        if (customContainerConfiguration.Port.HasValue)
        {
            containerBuilder = containerBuilder.WithPortBinding(customContainerConfiguration.Port.Value, true);
            containerBuilder = containerBuilder.WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(customContainerConfiguration.Port.Value));
        }
        else
            containerBuilder = containerBuilder.WithWaitStrategy(Wait.ForUnixContainer());

        var container = containerBuilder.Build();
        await container.StartAsync().ConfigureAwait(false);
        return container;
    }
}
