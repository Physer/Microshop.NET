using DotNet.Testcontainers.Builders;
using IntegrationTests.Configuration;
using MassTransit.Testing;
using Xunit;

namespace IntegrationTests;

public class ProductsServiceTestsFixture : IAsyncLifetime
{
    public ITestHarness? TestHarness { get; private set; }

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
        Dictionary<string, string?> configuration = new()
        {
            ["Servicebus:BaseUrl"] = RabbitMqContainerConfiguration.Hostname,
            ["Servicebus:ManagementUsername"] = RabbitMqContainerConfiguration.Username,
            ["Servicebus:ManagementPassword"] = RabbitMqContainerConfiguration.Password,
            ["Servicebus:Port"] = rabbitMqContainerPort.ToString()
        };
        var application = new ProductsServiceWebApplicationFactory(configuration);
        TestHarness = application.Services.GetTestHarness();
    }
}
