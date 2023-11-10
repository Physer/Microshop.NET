using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using DotNet.Testcontainers.Containers;
using InlineWebApplicationFactory;
using IntegrationTests.Configuration;
using IntegrationTests.Utilities;
using Microshop.ContainerConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Web.Pages;
using Xunit;

namespace IntegrationTests;

public class AuthenticationFixture : IAsyncLifetime
{
    private string? _externalAuthenticationServiceUrl;
    private IContainer? _authenticationServiceContainer;

    public InlineWebApplicationFactory<Program>? ValidApplicationFactory { get; set; }
    public InlineWebApplicationFactory<Program>? ApplicationFactoryWithInvalidAuthenticationService { get; set; }
    internal FakeUser AdminUser { get; set; }
    internal FakeUser ForbiddenUser { get; set; }

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
        _authenticationServiceContainer = await ContainerFactory.InitializeCustomContainer(authenticationServiceConfiguration);
        var authenticationServiceExternalPort = _authenticationServiceContainer.GetMappedPublicPort(authenticationServiceConfiguration.Port!.Value);
        _externalAuthenticationServiceUrl = $"http://localhost:{authenticationServiceExternalPort}";

        Dictionary<string, string?> validConfiguration = new()
        {
            { "Authentication:BaseUrl", _externalAuthenticationServiceUrl },
            { "UserManagement:BaseUrl", $"http://localhost:{authenticationCoreExternalPort}" },
            { "DataManagement:BaseUrl", $"http://localhost" }
        };
        ValidApplicationFactory = new InlineWebApplicationFactory<Program>(validConfiguration);
        Dictionary<string, string?> invalidConfiguration = new()
        {
            { "Authentication:BaseUrl", "http://invalid-hostname:1337" }
        };
        ApplicationFactoryWithInvalidAuthenticationService = new InlineWebApplicationFactory<Program>(invalidConfiguration);

        AdminUser = new("admin_integration_tests@microshop.local", Constants.DefaultPasswordValue);
        ForbiddenUser = new("forbidden_integration_tests@microshop.local", Constants.DefaultPasswordValue);
        await CreateIntegrationTestsUser(AdminUser, true);
        await CreateIntegrationTestsUser(ForbiddenUser, false);
    }

    public static async Task<HttpResponseMessage> SendSignInRequestAsync(HttpClient client, string? username, string? password)
    {
        var signInPage = await client.GetAsync("/signin");
        var content = await HtmlHelpers.GetDocumentAsync(signInPage);
        var form = content.QuerySelector<IHtmlFormElement>("form") ?? throw new Exception("Unable to find the sign in form");
        var submitButton = content.QuerySelector<IHtmlInputElement>("input[id='signInButton']") ?? throw new Exception("Unable to find the submit button on the sign in form");
        List<KeyValuePair<string, string?>> formValues = new()
        {
            { new(nameof(SignInModel.Username), username) },
            { new(nameof(SignInModel.Password), password) }
        };
        return await client.SendAsync(form, submitButton, formValues);
    }

    private async Task CreateIntegrationTestsUser(FakeUser userData, bool hasAdminRights)
    {
        if (string.IsNullOrWhiteSpace(_externalAuthenticationServiceUrl) || ValidApplicationFactory is null)
            throw new UninitializedTestFixtureException();

        var requestObject = new
        {
            FormFields = new object[]
            {
                new
                {
                    Id = "email",
                    Value = userData.Username
                },
                new
                {
                    Id = "password",
                    Value = userData.Password
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

        var httpClientFactory = ValidApplicationFactory.Services.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Unable to create a user for the Integration Tests");
    }
}
