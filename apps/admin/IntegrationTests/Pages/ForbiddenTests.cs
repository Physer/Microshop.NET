using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using IntegrationTests.Utilities;
using Shouldly;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AdminCollectionFixture))]
public class ForbiddenTests(AdminFixture fixture)
{
    private readonly AdminFixture _fixture = fixture;
    private const string _forbiddenUrl = "/forbidden";

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
        content.Body?.InnerHtml.ShouldContain(expectedErrorMessage);
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
        response.RequestMessage?.RequestUri?.PathAndQuery.ShouldBeEquivalentTo("/signout");
    }
}
