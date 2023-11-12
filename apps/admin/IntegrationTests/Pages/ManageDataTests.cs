using FluentAssertions;
using IntegrationTests.Utilities;
using System.Net;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AuthenticationCollectionFixture))]
public class ManageDataTests
{
    private readonly AuthenticationFixture _fixture;

    public ManageDataTests(AuthenticationFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task ManageDataPage_WithAdminUser_ShowsContent()
    {
        // Arrange
        var expectedHeader = "Use this page to perform actions on the data of Microshop.NET";
        var expectedGenerateDataContent = "Generate data";
        var expectedClearDataContent = "Clear data";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await _fixture.SendSignInRequestForAdminUserAsync(client);

        // Act
        var response = await client.GetAsync("/ManageData");
        var document = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        document.Body?.InnerHtml.Should().Contain(expectedHeader);
        document.Body?.InnerHtml.Should().Contain(expectedGenerateDataContent);
        document.Body?.InnerHtml.Should().Contain(expectedClearDataContent);
    }
}
