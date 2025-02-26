using IntegrationTests.Data;
using Shouldly;
using System.Net;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AdminCollectionFixture))]
public class AuthenticationTests(AdminFixture fixture)
{
    private readonly AdminFixture _fixture = fixture;

    [Theory]
    [ClassData(typeof(ProtectedPagesClassData))]
    public async Task ProtectedPages_ForAnonymousUser_RedirectsToSignin(string url)
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.ShouldNotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.ShouldBeEquivalentTo("/signin");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(ProtectedPagesClassData))]
    public async Task ProtectedPages_ForUserWithoutPermissions_RedirectsToForbidden(string url)
    {
        // Arrange
        var applicationFactory = _fixture.ValidApplicationFactory!;
        var client = applicationFactory.CreateClient();
        _ = await _fixture.SendSignInRequestForForbiddenUserAsync(client);

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.ShouldNotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.ShouldBeEquivalentTo("/forbidden");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }
}
