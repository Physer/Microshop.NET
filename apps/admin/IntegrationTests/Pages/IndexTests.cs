using FluentAssertions;
using IntegrationTests.Data;
using IntegrationTests.Utilities;
using System.Net;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AuthenticationCollectionFixture))]
public class IndexTests
{
    private readonly AuthenticationFixture _fixture;
    private const string _signInUrl = "/signin";

    public IndexTests(AuthenticationFixture fixture) => _fixture = fixture;

    [Theory]
    [ClassData(typeof(IndexUrlsClassData))]
    public async Task IndexPage_ForAnonymousUser_RedirectsToSignIn(string url)
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

    [Theory]
    [ClassData(typeof(IndexUrlsClassData))]
    public async Task IndexPage_ForNonAdminUser_RedirectsToForbidden(string url)
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await AuthenticationFixture.SendSignInRequestAsync(client, _fixture.ForbiddenUser.Username, _fixture.ForbiddenUser.Password);

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo("/forbidden");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(IndexUrlsClassData))]
    public async Task IndexPage_ForAdminUser_ShowsIndex(string url)
    {
        // Arrange
        var expectedContent = "Admin portal";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await AuthenticationFixture.SendSignInRequestAsync(client, _fixture.AdminUser.Username, _fixture.AdminUser.Password);

        // Act
        var response = await client.GetAsync(url);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        document.Body?.InnerHtml.Should().Contain(expectedContent);
    }
}
