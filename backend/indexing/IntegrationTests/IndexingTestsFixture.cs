using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using IntegrationTests.Configuration;
using MassTransit.Testing;
using Xunit;

namespace IntegrationTests;

public class IndexingTestsFixture : IAsyncLifetime
{
    public ITestHarness? TestHarness { get; private set; }
    public IContainer? MeilisearchContainer { get; private set; }

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        var rabbitMqConfiguration = new RabbitMqContainerConfiguration();
        var rabbitMqContainer = new ContainerBuilder()
            .WithImage(rabbitMqConfiguration.ImageName)
            .WithEnvironment(rabbitMqConfiguration.EnvironmentVariables)
            .WithPortBinding(RabbitMqContainerConfiguration.Port, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilMessageIsLogged(@"[s|S]erver startup complete")
                .UntilPortIsAvailable(RabbitMqContainerConfiguration.Port))
            .Build();
        await rabbitMqContainer.StartAsync().ConfigureAwait(false);

        var meilisearchConfiguration = new MeilisearchContainerConfiguration();
        MeilisearchContainer = new ContainerBuilder()
            .WithImage(meilisearchConfiguration.ImageName)
            .WithEnvironment(meilisearchConfiguration.EnvironmentVariables)
            .WithPortBinding(MeilisearchContainerConfiguration.Port, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(MeilisearchContainerConfiguration.Port))
            .Build();
        await MeilisearchContainer.StartAsync().ConfigureAwait(false);

        var rabbitMqContainerPort = rabbitMqContainer.GetMappedPublicPort(RabbitMqContainerConfiguration.Port);
        var meilisearchContainerPort = MeilisearchContainer.GetMappedPublicPort(MeilisearchContainerConfiguration.Port);
        Dictionary<string, string?> configuration = new()
        {
            ["Indexing:BaseUrl"] = $"http://localhost:{meilisearchContainerPort}/",
            ["Indexing:ApiKey"] = MeilisearchContainerConfiguration.ApiKey,
            ["Indexing:IndexingIntervalInSeconds"] = MeilisearchContainerConfiguration.IntervalInSeconds.ToString(),
            ["Servicebus:BaseUrl"] = RabbitMqContainerConfiguration.Hostname,
            ["Servicebus:ManagementUsername"] = RabbitMqContainerConfiguration.Username,
            ["Servicebus:ManagementPassword"] = RabbitMqContainerConfiguration.Password,
            ["Servicebus:Port"] = rabbitMqContainerPort.ToString()
        };
        var application = new IndexingWebApplicationFactory(configuration);
        TestHarness = application.Services.GetTestHarness();
    }
}
