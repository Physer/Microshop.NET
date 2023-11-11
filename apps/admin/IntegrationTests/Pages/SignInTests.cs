using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using IntegrationTests.Utilities;
using System.Net;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AuthenticationCollectionFixture))]
public class SignInTests
{
    private readonly AuthenticationFixture _fixture;
    private readonly string _signInUrl;

    public SignInTests(AuthenticationFixture fixture)
    {
        _fixture = fixture;
        _signInUrl = "/signin";
    }

    [Fact]
    public async Task SignInPage_ForAnonymousUser_ReturnsOk()
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(_signInUrl);

        // Assert
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo(_signInUrl);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    [InlineData("/ManageData")]
    [InlineData("/ManageUsers")]
    public async Task ProtectedPages_ForAnonymousUser_RedirectsToSignin(string url)
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo(_signInUrl);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SignInPage_WithWrongCredentials_ShowsError()
    {
        // Arrange
        var expectedErrorMessage = "Invalid credentials or permissions";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await AuthenticationFixture.SendSignInRequestAsync(client, Constants.DefaultTextValue, Constants.DefaultTextValue);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);
        var errorAlert = responseContent.QuerySelector<IHtmlDivElement>("div[id='errorAlert']");

        // Assert
        errorAlert?.InnerHtml.Should().Be(expectedErrorMessage);
    }

    [Fact]
    public async Task SignInPage_WithValidCredentialsAndInvalidPermissions_RedirectsToForbidden()
    {
        // Arrange
        var expectedResponseUrl = "http://localhost/Forbidden";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await AuthenticationFixture.SendSignInRequestAsync(client, _fixture.ForbiddenUser.Username, _fixture.ForbiddenUser.Password);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        responseContent.Url.Should().Be(expectedResponseUrl);
    }

    [Fact]
    public async Task SignInPage_WithValidCredentialsAndValidPermissions_RedirectsToIndex()
    {
        // Arrange
        var expectedResponseUrl = "http://localhost/";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await AuthenticationFixture.SendSignInRequestAsync(client, _fixture.AdminUser.Username, _fixture.AdminUser.Password);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        responseContent.Url.Should().Be(expectedResponseUrl);
    }

    [Theory]
    [InlineData("username", "")]
    [InlineData("username", " ")]
    [InlineData("username", null)]
    [InlineData("", "password")]
    [InlineData(" ", "password")]
    [InlineData(null, "password")]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData(null, null)]
    public async Task SignInPage_WithInvalidFormData_ReturnsToSignInPage(string? username, string? password)
    {
        // Arrange
        var expectedUrl = $"http://localhost{_signInUrl}";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await AuthenticationFixture.SendSignInRequestAsync(client, username, password);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        responseContent.Url.Should().Be(expectedUrl);
    }

    [Fact]
    public async Task SignInPage_WithAuthenticationServiceFailure_ShowsGenericError()
    {
        // Arrange
        var expectedErrorMessage = "Something went wrong, please try again later";
        var applicationFactory = _fixture.ApplicationFactoryWithInvalidAuthenticationService!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await AuthenticationFixture.SendSignInRequestAsync(client, Constants.DefaultTextValue, Constants.DefaultTextValue);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);
        var errorAlert = responseContent.QuerySelector<IHtmlDivElement>("div[id='errorAlert']");

        // Assert
        errorAlert?.InnerHtml.Should().Be(expectedErrorMessage);
    }
}
