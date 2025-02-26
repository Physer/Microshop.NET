using IntegrationTests.Utilities;
using Shouldly;
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
        response.ShouldNotBeNull();
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        document.Body?.InnerHtml.ShouldContain(expectedContent);
    }
}
