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
        var authenticationCoreInternalPort = authenticationCoreConfiguration.Port!.Value;
        var authenticationCoreExternalPort = authenticationCoreContainer.GetMappedPublicPort(authenticationCoreInternalPort);

        AuthenticationServiceConfiguration authenticationServiceConfiguration = new()
        {
            AuthenticationCoreIpAddress = authenticationCoreIpAddress,
            AuthenticationCorePort = authenticationCoreInternalPort
        };
        var authenticationServiceContainer = await ContainerFactory.InitializeCustomContainer(authenticationServiceConfiguration);
        var authenticationServiceExternalPort = authenticationServiceContainer.GetMappedPublicPort(authenticationServiceConfiguration.Port!.Value);

        Dictionary<string, string?> configuration = new()
        {
            { "Authentication:BaseUrl", $"http://localhost:{authenticationServiceExternalPort}" },
            { "UserManagement:BaseUrl", $"http://localhost:{authenticationCoreExternalPort}" },
            { "DataManagement:BaseUrl", $"http://localhost" }
        };
        ApplicationFactory = new InlineWebApplicationFactory<Program>(configuration);
    }
}
