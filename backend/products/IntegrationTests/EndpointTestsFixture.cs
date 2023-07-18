using Application.Options;
using DotNet.Testcontainers.Builders;
using IntegrationTests.Configuration;
using Xunit;

namespace IntegrationTests;

public class EndpointTestsFixture : IAsyncLifetime
{
    public ServicebusOptions? ServicebusOptions { get; private set; }

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

        ServicebusOptions = new()
        {
            BaseUrl = "localhost",
            ManagementUsername = RabbitMqContainerConfiguration.Username,
            ManagementPassword = RabbitMqContainerConfiguration.Password,
            Port = rabbitMqContainerPort
        };
    }
}
