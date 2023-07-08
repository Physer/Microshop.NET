﻿using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using IntegrationTests.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit;

namespace IntegrationTests;

public class EndpointTests : IAsyncLifetime
{
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
            .WithPortBinding(RabbitMqContainerConfiguration.Port, true)
            .WithWaitStrategy(Wait
                .ForUnixContainer()
                .UntilMessageIsLogged(@"[s|S]erver startup complete")
                .UntilPortIsAvailable(RabbitMqContainerConfiguration.Port))
            .Build();
        await _rabbitMqContainer.StartAsync().ConfigureAwait(false);
        _rabbitMqContainerPort = _rabbitMqContainer.GetMappedPublicPort(RabbitMqContainerConfiguration.Port);
    }

    [Fact]
    public async Task GenerateProducts_ReturnsOkAndMessageId()
    {
        // Arrange
        Dictionary<string, string?> configuration = new()
        {
            ["Servicebus:BaseUrl"] = "localhost",
            ["Servicebus:ManagementUsername"] = RabbitMqContainerConfiguration.Username,
            ["Servicebus:ManagementPassword"] = RabbitMqContainerConfiguration.Password,
            ["Servicebus:Port"] = _rabbitMqContainerPort.ToString()
        };

        var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder => builder.ConfigureAppConfiguration((_, configurationBuilder) => configurationBuilder.AddInMemoryCollection(configuration)));
        var client = factory.CreateClient();

        // Act
        var response = await client.PostAsync("/products", default);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();
        Guid.TryParse(content, out var parsedGuid).Should().BeTrue();
        parsedGuid.Should().NotBe(Guid.Empty);
    }
}
