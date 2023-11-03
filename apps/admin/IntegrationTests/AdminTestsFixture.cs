using InlineWebApplicationFactory;
using IntegrationTests.Configuration;
using Microshop.ContainerConfiguration;
using Xunit;

namespace IntegrationTests;

public class AdminTestsFixture : IAsyncLifetime
{
    public InlineWebApplicationFactory<Program>? ApplicationFactory { get; set; }

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        AuthenticationDatabaseConfiguration authenticationDatabaseConfiguration = new();
        var authenticationDatabaseContainer = await ContainerFactory.InitializeCustomContainer(authenticationDatabaseConfiguration);

        AuthenticationCoreConfiguration authenticationCoreConfiguration = new()
        {
            AuthenticationDatabaseIpAddress = authenticationDatabaseContainer.IpAddress,
            AuthenticationDatabasePort = authenticationDatabaseConfiguration.Port!.Value
        };
        var authenticationCoreContainer = await ContainerFactory.InitializeCustomContainer(authenticationCoreConfiguration);
        var authenticationCoreIpAddress = authenticationCoreContainer.IpAddress;
        var authenticationCorePort = authenticationCoreConfiguration.Port!.Value;

        AuthenticationServiceConfiguration authenticationServiceConfiguration = new() 
        {
            AuthenticationCoreIpAddress = authenticationCoreIpAddress,
            AuthenticationCorePort = authenticationCorePort
        };
        var authenticationServiceContainer = await ContainerFactory.InitializeCustomContainer(authenticationServiceConfiguration);
        var authenticationServiceIpAddress = authenticationServiceContainer.IpAddress;
        var authenticationServicePort = authenticationServiceConfiguration.Port!.Value;

        Dictionary<string, string?> configuration = new()
        {
            { "Authentication:BaseUrl", $"http://{authenticationServiceIpAddress}:{authenticationServicePort}" },
            { "UserManagement:BaseUrl", $"http://{authenticationCoreIpAddress}:{authenticationCorePort}" },
            { "DataManagement:BaseUrl", $"http://localhost" }
        };
        ApplicationFactory = new InlineWebApplicationFactory<Program>(configuration);
    }
}
