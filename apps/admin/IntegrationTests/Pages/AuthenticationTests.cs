using FluentAssertions;
using IntegrationTests.Data;
using System.Net;
using Xunit;

namespace IntegrationTests.Pages;

[Collection(nameof(AdminCollectionFixture))]
public class AuthenticationTests
{
    private readonly AdminFixture _fixture;

    public AuthenticationTests(AdminFixture fixture) => _fixture = fixture;

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
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo("/signin");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
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
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo("/forbidden");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
