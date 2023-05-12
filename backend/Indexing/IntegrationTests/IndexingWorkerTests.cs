using DotNet.Testcontainers.Builders;
using IntegrationTests.Configuration;
using Xunit;

namespace IntegrationTests;

public class IndexingWorkerTests : IAsyncLifetime
{
    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        var rabbitMqConfiguration = new RabbitMqContainerConfiguration();
        var rabbitMqContainer = new ContainerBuilder()
            .WithImage(rabbitMqConfiguration.ImageName)
            .WithEnvironment(rabbitMqConfiguration.EnvironmentVariables)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilMessageIsLogged(@"[s|S]erver startup complete"))
            .Build();
        await rabbitMqContainer.StartAsync().ConfigureAwait(false);

        var meilisearchConfiguration = new MeilisearchContainerConfiguration();
        var meilisearchContainer = new ContainerBuilder()
            .WithImage(meilisearchConfiguration.ImageName)
            .WithEnvironment(meilisearchConfiguration.EnvironmentVariables)
            .WithWaitStrategy(Wait.ForUnixContainer())
            .Build();
        await meilisearchContainer.StartAsync().ConfigureAwait(false);

        // rabbitMqContainer.Name.Split('/').Last()
        var productsServiceConfiguration = new MicroshopProductsContainerConfiguration(rabbitMqContainer.IpAddress, rabbitMqConfiguration.Username, rabbitMqConfiguration.Password);
        var productsServiceContainer = new ContainerBuilder()
            .WithImage(productsServiceConfiguration.ImageName)
            .WithEnvironment(productsServiceConfiguration.EnvironmentVariables)
            .WithWaitStrategy(Wait.ForUnixContainer())
            .Build();
        await productsServiceContainer.StartAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task TestIndexingWorker()
    {
        // Arrange

        // Act

        // Assert
    }
}
