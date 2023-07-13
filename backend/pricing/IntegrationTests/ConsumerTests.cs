using Application.Configuration;
using Application.Options;
using AutoFixture.Xunit2;
using Domain;
using DotNet.Testcontainers.Builders;
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
    private ServicebusOptions? _servicebusOptions;

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
        var rabbitMqContainerPort = rabbitMqContainer.GetMappedPublicPort(RabbitMqContainerConfiguration.Port);

        _servicebusOptions = new()
        {
            BaseUrl = "localhost",
            ManagementUsername = RabbitMqContainerConfiguration.Username,
            ManagementPassword = RabbitMqContainerConfiguration.Username,
            Port = rabbitMqContainerPort
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

        // Act
        await testHarness.Start();
        await testHarness.Bus.Publish<ProductsGenerated>(new(products));
        await testHarness.Stop();

        // Assert
        (await testHarness.Consumed.Any<ProductsGenerated>()).Should().BeTrue();
        (await testHarness.Published.Any<PricesGenerated>()).Should().BeTrue();
    }
}
