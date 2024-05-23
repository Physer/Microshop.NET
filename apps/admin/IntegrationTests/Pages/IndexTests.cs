using FluentAssertions;
using IntegrationTests.Utilities;
using System.Net;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AdminCollectionFixture))]
public class IndexTests(AdminFixture fixture)
{
    private readonly AdminFixture _fixture = fixture;
    private const string _signInUrl = "/signin";

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    public async Task IndexPage_ForAdminUser_ShowsIndex(string url)
    {
        // Arrange
        var expectedContent = "Admin portal";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await _fixture.SendSignInRequestForAdminUserAsync(client);

        // Act
        var response = await client.GetAsync(url);
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        document.Body?.InnerHtml.Should().Contain(expectedContent);
    }
}
