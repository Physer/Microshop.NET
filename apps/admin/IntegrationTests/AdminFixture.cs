using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using InlineWebApplicationFactory;
using IntegrationTests.Utilities;
using Microshop.ContainerConfiguration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using Web.Pages;
using Xunit;

namespace IntegrationTests;

public class AdminFixture : IAsyncLifetime
{
    private string? _externalAuthenticationServiceUrl;

    public InlineWebApplicationFactory<Program>? ValidApplicationFactory { get; set; }
    public InlineWebApplicationFactory<Program>? ApplicationFactoryWithInvalidAuthenticationService { get; set; }
    private FakeUser? _adminUser;
    private FakeUser? _forbiddenUser;
    private string? _adminKey;

    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync()
    {
        await ContainerFactory.InitializeAllPredefinedContainers();
        var supertokensData = ContainerFactory.SupertokensContainerConfiguration!;
        var supertokensPort = supertokensData.Container.GetMappedPublicPort(supertokensData.Configuration.Port!.Value);

        var microshopApiData = ContainerFactory.MicroshopApiContainerConfiguration!;
        var microshopApiPort = microshopApiData.Container.GetMappedPublicPort(microshopApiData.Configuration.Port!.Value);

        var authenticationServiceData = ContainerFactory.AuthenticationServiceContainerConfiguration!;
        var authenticationServicePort = authenticationServiceData.Container.GetMappedPublicPort(authenticationServiceData.Configuration.Port!.Value);
        _externalAuthenticationServiceUrl = $"http://localhost:{authenticationServicePort}";

        Dictionary<string, string?> validConfiguration = new()
        {
            { "Authentication:BaseUrl", _externalAuthenticationServiceUrl },
            { "UserManagement:BaseUrl", $"http://localhost:{supertokensPort}" },
            { "DataManagement:BaseUrl", $"http://localhost:{microshopApiPort}" }
        };
        ValidApplicationFactory = new InlineWebApplicationFactory<Program>(validConfiguration);
        Dictionary<string, string?> invalidConfiguration = new()
        {
            { "Authentication:BaseUrl", "http://invalid-hostname:1337" }
        };
        ApplicationFactoryWithInvalidAuthenticationService = new InlineWebApplicationFactory<Program>(invalidConfiguration);

        _adminKey = authenticationServiceData.Configuration.AdminKey;
        _adminUser = new("admin_integration_tests@microshop.local", Constants.DefaultPasswordValue);
        _forbiddenUser = new("forbidden_integration_tests@microshop.local", Constants.DefaultPasswordValue);
        await CreateIntegrationTestsUserAsync(_adminUser, true);
        await CreateIntegrationTestsUserAsync(_forbiddenUser, false);
    }

    public async Task<HttpResponseMessage> SendSignInRequestForAdminUserAsync(HttpClient client) => await SendSignInRequestAsync(client, _adminUser);

    public async Task<HttpResponseMessage> SendSignInRequestForForbiddenUserAsync(HttpClient client) => await SendSignInRequestAsync(client, _forbiddenUser);

    public static async Task<HttpResponseMessage> SendSignInRequestForInvalidUserAsync(HttpClient client) => await SendSignInRequestAsync(client, new(Constants.DefaultTextValue, Constants.DefaultTextValue));

    public static async Task<HttpResponseMessage> SendSignInRequestForCustomUserAsync(HttpClient client, string? username, string? password) => await SendSignInRequestAsync(client, new(username, password));

    private static async Task<HttpResponseMessage> SendSignInRequestAsync(HttpClient client, FakeUser? userData)
    {
        var signInPage = await client.GetAsync("/signin");
        var content = await HtmlHelpers.GetDocumentAsync(signInPage);
        var form = content.QuerySelector<IHtmlFormElement>("form") ?? throw new Exception("Unable to find the sign in form");
        var submitButton = content.QuerySelector<IHtmlInputElement>("input[id='signInButton']") ?? throw new Exception("Unable to find the submit button on the sign in form");
        List<KeyValuePair<string, string?>> formValues = new()
        {
            { new(nameof(SignInModel.Username), userData?.Username) },
            { new(nameof(SignInModel.Password), userData?.Password) }
        };
        return await client.SendAsync(form, submitButton, formValues);
    }

    private async Task CreateIntegrationTestsUserAsync(FakeUser userData, bool hasAdminRights)
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
            requestMessage.Headers.Add(Constants.AdminKeyHeader, _adminKey);

        var httpClientFactory = ValidApplicationFactory.Services.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(requestMessage);
        if (!response.IsSuccessStatusCode)
            throw new Exception("Unable to create a user for the Integration Tests");
    }
}
