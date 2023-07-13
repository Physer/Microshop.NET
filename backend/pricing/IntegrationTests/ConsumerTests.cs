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
    private IContainer? _rabbitMqContainer;
    private RabbitMqContainerConfiguration? _rabbitMqConfiguration;
    private int? _rabbitMqContainerPort;
    private ServicebusOptions? _servicebusOptions;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        _rabbitMqConfiguration = new RabbitMqContainerConfiguration();
        _rabbitMqContainer = new ContainerBuilder()
            .WithImage(_rabbitMqConfiguration.ImageName)
            .WithEnvironment(_rabbitMqConfiguration.EnvironmentVariables)
            .WithPortBinding(RabbitMqContainerConfiguration.Port, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilMessageIsLogged(@"[s|S]erver startup complete")
                .UntilPortIsAvailable(RabbitMqContainerConfiguration.Port))
            .Build();
        await _rabbitMqContainer.StartAsync().ConfigureAwait(false);
        _rabbitMqContainerPort = _rabbitMqContainer.GetMappedPublicPort(RabbitMqContainerConfiguration.Port);

        _servicebusOptions = new()
        {
            BaseUrl = "localhost",
            ManagementUsername = RabbitMqContainerConfiguration.Username,
            ManagementPassword = RabbitMqContainerConfiguration.Username,
            Port = _rabbitMqContainerPort ?? 0
        };
    }

    [Theory]
    [AutoData]
    public async Task Consumer_ProcessesMessageAndPublishesMessage(IEnumerable<Product> products)
    {
        // Arrange
        Dictionary<string, string?> configuration = new()
        {
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
        var productsGeneratedConsumerHarness = testHarness.GetConsumerHarness<ProductsGeneratedConsumer>();
        var hasPublishedMessage = false;

        // Act
        await testHarness.Bus.Publish<ProductsGenerated>(new(products), _ => hasPublishedMessage = true);

        // Assert
        if (hasPublishedMessage)
            (await productsGeneratedConsumerHarness.Consumed.Any<ProductsGenerated>()).Should().BeTrue();
        else
            throw new MassTransitException("Unable to publish message and verify the consumer has consumed the message");
    }
}
