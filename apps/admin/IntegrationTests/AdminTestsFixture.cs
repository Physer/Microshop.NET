using InlineWebApplicationFactory;
using IntegrationTests.Configuration;
using Microshop.ContainerConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Xunit;

namespace IntegrationTests;

public class AdminTestsFixture : IAsyncLifetime
{
    private string? _externalAuthenticationServiceUrl;

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
        _externalAuthenticationServiceUrl = $"http://localhost:{authenticationServiceExternalPort}";

        Dictionary<string, string?> configuration = new()
        {
            { "Authentication:BaseUrl", _externalAuthenticationServiceUrl },
            { "UserManagement:BaseUrl", $"http://localhost:{authenticationCoreExternalPort}" },
            { "DataManagement:BaseUrl", $"http://localhost" }
        };
        ApplicationFactory = new InlineWebApplicationFactory<Program>(configuration);
    }

    public async Task CreateIntegrationTestsUser(string username, string password, bool hasAdminRights)
    {
        if (string.IsNullOrWhiteSpace(_externalAuthenticationServiceUrl) || ApplicationFactory is null)
            throw new Exception("Test suite has not been initialized");

        var requestObject = new
        {
            FormFields = new object[]
            {
                new
                {
                    Id = "email",
                    Value = username
                },
                new
                {
                    Id = "password",
                    Value = password
                }
            }
        };
        var serializedRequestObject = JsonSerializer.Serialize(requestObject, Constants.DefaultJsonSerializerOptions);
        var stringContent = new StringContent(serializedRequestObject);
        HttpRequestMessage requestMessage = new(HttpMethod.Post, $"{_externalAuthenticationServiceUrl}/auth/signup")
        {
            Content = stringContent
        };
        if (hasAdminRights)
            requestMessage.Headers.Add(Constants.AdminKeyHeader, Constants.DefaultTextValue);

        var httpClientFactory = ApplicationFactory.Services.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Unable to create a user for the Integration Tests");
    }
}
