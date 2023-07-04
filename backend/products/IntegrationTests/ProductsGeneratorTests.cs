using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using FluentAssertions;
using IntegrationTests.Configuration;
using Xunit;

namespace IntegrationTests;

public class ProductsGeneratorTests : IAsyncLifetime
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
    public async Task ProductsGenerator_GeneratesProductsAndSendsEventSuccesfully()
    {
        // Arrange
        Dictionary<string, string?> configuration = new()
        {
            { "Servicebus:BaseUrl", "localhost" },
            { "Servicebus:ManagementUsername", RabbitMqContainerConfiguration.Username },
            { "Servicebus:ManagementPassword", RabbitMqContainerConfiguration.Password },
            { "Servicebus:Port", _rabbitMqContainerPort.ToString() }
        };
        var commandlineConfiguration = configuration.Select(c => $"--{c.Key}={c.Value}");

        // Act
        var exception = await Record.ExceptionAsync(() => ProductsGenerator.Program.Main(commandlineConfiguration.ToArray()));

        // Assert
        exception.Should().BeNull();
    }
}
