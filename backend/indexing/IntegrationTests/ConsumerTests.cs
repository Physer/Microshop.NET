using Application.Configuration;
using Application.Options;
using AutoFixture.Xunit2;
using Domain;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using IntegrationTests.Configuration;
using MassTransit;
using MassTransit.Testing;
using Messaging;
using Messaging.Messages;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace IntegrationTests;

public class ConsumerTests : IAsyncLifetime
{
    private IContainer? _meilisearchContainer;
    private ServicebusOptions? _servicebusOptions;
    private IndexingOptions? _indexingOptions;

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
        _meilisearchContainer = new ContainerBuilder()
            .WithImage(meilisearchConfiguration.ImageName)
            .WithEnvironment(meilisearchConfiguration.EnvironmentVariables)
            .WithPortBinding(MeilisearchContainerConfiguration.Port, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilPortIsAvailable(MeilisearchContainerConfiguration.Port))
            .Build();
        await _meilisearchContainer.StartAsync().ConfigureAwait(false);

        var rabbitMqContainerPort = rabbitMqContainer.GetMappedPublicPort(RabbitMqContainerConfiguration.Port);
        var meilisearchContainerPort = _meilisearchContainer.GetMappedPublicPort(MeilisearchContainerConfiguration.Port);

        _servicebusOptions = new()
        {
            BaseUrl = RabbitMqContainerConfiguration.Hostname,
            ManagementUsername = RabbitMqContainerConfiguration.Username,
            ManagementPassword = RabbitMqContainerConfiguration.Password,
            Port = rabbitMqContainerPort
        };
        _indexingOptions = new()
        {
            BaseUrl = $"http://localhost:{meilisearchContainerPort}/",
            ApiKey = MeilisearchContainerConfiguration.ApiKey,
            IndexingIntervalInSeconds = MeilisearchContainerConfiguration.IntervalInSeconds
        };
    }

    [Theory]
    [AutoData]
    public async void ReceivesMessage_ProductsGenerated_IndexesProducts(IEnumerable<Product> products)
    {
        // Arrange
        Dictionary<string, string?> configuration = new()
        {
            ["Indexing:BaseUrl"] = _indexingOptions!.BaseUrl,
            ["Indexing:ApiKey"] = _indexingOptions!.ApiKey,
            ["Indexing:IndexingIntervalInSeconds"] = _indexingOptions!.IndexingIntervalInSeconds.ToString(),
            ["Servicebus:BaseUrl"] = _servicebusOptions!.BaseUrl,
            ["Servicebus:ManagementUsername"] = _servicebusOptions!.ManagementUsername,
            ["Servicebus:ManagementPassword"] = _servicebusOptions!.ManagementPassword,
            ["Servicebus:Port"] = _servicebusOptions!.Port.ToString()
        };
        TestConfiguration.Create(builder => builder.AddInMemoryCollection(configuration));
        var application = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddMassTransitTestHarness(cfg => cfg.ConfigureBusRegistration(_servicebusOptions));
                });
            });
        var testHarness = application.Services.GetTestHarness();

        // Act
        await testHarness.Start();
        await testHarness.Bus.Publish<ProductsGenerated>(new(products));
        await testHarness.Stop();

        // Assert
        var containerLogs = await _meilisearchContainer!.GetLogsAsync();
        containerLogs.Should().NotBeNull();
        containerLogs.Stderr.Should().Contain("indexed_documents:");
        containerLogs.Stderr.Should().NotContain("indexed_documents: 0");
    }
}
