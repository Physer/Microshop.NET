using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using IntegrationTests.Utilities;
using Shouldly;
using System.Net;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AdminCollectionFixture))]
public class SignInTests(AdminFixture fixture)
{
    private readonly AdminFixture _fixture = fixture;
    private readonly string _signInUrl = "/signin";

    [Fact]
    public async Task SignInPage_ForAnonymousUser_ReturnsOk()
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(_signInUrl);

        // Assert
        response.ShouldNotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.ShouldBeEquivalentTo(_signInUrl);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SignInPage_WithWrongCredentials_ShowsError()
    {
        // Arrange
        var expectedErrorMessage = "Invalid credentials or permissions";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await AdminFixture.SendSignInRequestForInvalidUserAsync(client);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);
        var errorAlert = responseContent.QuerySelector<IHtmlDivElement>("div[id='errorAlert']");

        // Assert
        errorAlert?.InnerHtml.ShouldBe(expectedErrorMessage);
    }

    [Fact]
    public async Task SignInPage_WithValidCredentialsAndInvalidPermissions_RedirectsToForbidden()
    {
        // Arrange
        var expectedResponseUrl = "http://localhost/Forbidden";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await _fixture.SendSignInRequestForForbiddenUserAsync(client);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        responseContent.Url.ShouldBe(expectedResponseUrl);
    }

    [Fact]
    public async Task SignInPage_WithValidCredentialsAndValidPermissions_RedirectsToIndex()
    {
        // Arrange
        var expectedResponseUrl = "http://localhost/";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await _fixture.SendSignInRequestForAdminUserAsync(client);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        responseContent.Url.ShouldBe(expectedResponseUrl);
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
        var response = await AdminFixture.SendSignInRequestForCustomUserAsync(client, username, password);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        responseContent.Url.ShouldBe(expectedUrl);
    }

    [Fact]
    public async Task SignInPage_WithAuthenticationServiceFailure_ShowsGenericError()
    {
        // Arrange
        var expectedErrorMessage = "Something went wrong, please try again later";
        var applicationFactory = _fixture.ApplicationFactoryWithInvalidAuthenticationService!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await AdminFixture.SendSignInRequestForInvalidUserAsync(client);
        var responseContent = await HtmlHelpers.GetDocumentAsync(response);
        var errorAlert = responseContent.QuerySelector<IHtmlDivElement>("div[id='errorAlert']");

        // Assert
        errorAlert?.InnerHtml.ShouldBe(expectedErrorMessage);
    }
}
