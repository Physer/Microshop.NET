using FluentAssertions;
using IntegrationTests.Utilities;
using Xunit;

namespace IntegrationTests;

[Collection(nameof(AuthenticationCollectionFixture))]
public class ForbiddenTests
{
    private readonly AuthenticationFixture _fixture;

    public ForbiddenTests(AuthenticationFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task ForbiddenPage_ShowsMessage()
    {
        // Arrange
        var expectedErrorMessage = "You do not have administrative rights for Microshop.NET.";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/forbidden");
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        content.Body?.InnerHtml.Should().Contain(expectedErrorMessage);
    }
}
