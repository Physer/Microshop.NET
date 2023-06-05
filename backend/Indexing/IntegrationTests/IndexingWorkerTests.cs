using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using IntegrationTests.Builders;
using IntegrationTests.Configuration;
using Xunit;

namespace IntegrationTests;

public class IndexingWorkerTests : IAsyncLifetime
{
    private IContainer? _meilisearchContainer;
    private IContainer? _rabbitMqContainer;
    private RabbitMqContainerConfiguration? _rabbitMqConfiguration;
    private int? _rabbitMqContainerPort;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        _rabbitMqConfiguration = new RabbitMqContainerConfiguration();
        _rabbitMqContainer = new ContainerBuilder()
            .WithImage(_rabbitMqConfiguration.ImageName)
            .WithEnvironment(_rabbitMqConfiguration.EnvironmentVariables)
            .WithPortBinding(_rabbitMqConfiguration.Port, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilMessageIsLogged(@"[s|S]erver startup complete"))
            .Build();
        await _rabbitMqContainer.StartAsync().ConfigureAwait(false);

        var meilisearchConfiguration = new MeilisearchContainerConfiguration();
        _meilisearchContainer = new ContainerBuilder()
            .WithImage(meilisearchConfiguration.ImageName)
            .WithEnvironment(meilisearchConfiguration.EnvironmentVariables)
            .WithWaitStrategy(Wait.ForUnixContainer())
            .Build();
        await _meilisearchContainer.StartAsync().ConfigureAwait(false);

        _rabbitMqContainerPort = _rabbitMqContainer.GetMappedPublicPort(_rabbitMqConfiguration.Port);
        var productsServiceConfiguration = new MicroshopProductsContainerConfiguration(_rabbitMqContainer.IpAddress, _rabbitMqConfiguration.Username, _rabbitMqConfiguration.Password, _rabbitMqContainerPort.Value);
        var productsServiceContainer = new ContainerBuilder()
            .WithImage(productsServiceConfiguration.ImageName)
            .WithEnvironment(productsServiceConfiguration.EnvironmentVariables)
            .WithWaitStrategy(Wait.ForUnixContainer())
            .Build();
        await productsServiceContainer.StartAsync().ConfigureAwait(false);
    }

    [Fact]
    public async Task IndexingWorker_IndexesSuccesfully()
    {
        // Arrange
        Dictionary<string, string?> configuration = new()
        {
            { "Indexing:BaseUrl", $"http://{_meilisearchContainer?.IpAddress}:7700/" },
            { "Servicebus:BaseUrl", _rabbitMqContainer?.IpAddress },
            { "Servicebus:ManagementUsername", _rabbitMqConfiguration?.Username },
            { "Servicebus:ManagementPassword", _rabbitMqConfiguration?.Password },
            { "Servicebus:Port", _rabbitMqContainerPort.ToString() }
        };
        var host = new ApplicationBuilder(configuration)
            .Build();
        await host.StartAsync();
        Thread.Sleep(5000);
        await host.StopAsync();

        // Act

        // Assert
    }
}
