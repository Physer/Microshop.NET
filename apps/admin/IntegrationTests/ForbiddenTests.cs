using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using FluentAssertions;
using IntegrationTests.Utilities;
using Xunit;

namespace IntegrationTests;

[Collection(nameof(AuthenticationCollectionFixture))]
public class ForbiddenTests
{
    private readonly AuthenticationFixture _fixture;
    private const string _forbiddenUrl = "/forbidden";

    public ForbiddenTests(AuthenticationFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task ForbiddenPage_ShowsMessage()
    {
        // Arrange
        var expectedErrorMessage = "You do not have administrative rights for Microshop.NET.";
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(_forbiddenUrl);
        var content = await HtmlHelpers.GetDocumentAsync(response);

        // Assert
        content.Body?.InnerHtml.Should().Contain(expectedErrorMessage);
    }

    [Fact]
    public async Task ForbiddenPage_OnPost_RedirectsToSignOut()
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        var forbiddenPage = await client.GetAsync(_forbiddenUrl);
        var content = await HtmlHelpers.GetDocumentAsync(forbiddenPage);
        var form = content.QuerySelector<IHtmlFormElement>("form") ?? throw new Exception("Unable to find the sign in form");
        var submitButton = content.QuerySelector<IHtmlInputElement>("input[id='signOutButton']") ?? throw new Exception("Unable to find the submit button on the sign in form");

        // Act
        var response = await client.SendAsync(form, submitButton);

        // Assert
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo("/signout");
    }
}
