using DotNet.Testcontainers.Builders;
using IntegrationTests.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests;

public class ApiTestsFixture : IAsyncLifetime
{
    public WebApplicationFactory<Program>? ApplicationFactory { get; private set; }

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
        ApplicationFactory = new ApiWebApplicationFactory(configuration);
    }
}
