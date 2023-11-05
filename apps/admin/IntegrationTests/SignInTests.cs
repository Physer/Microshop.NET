using FluentAssertions;
using System.Net;
using Xunit;

namespace IntegrationTests;

public class SignInTests : IClassFixture<AdminTestsFixture>
{
    private readonly AdminTestsFixture _fixture;

    public SignInTests(AdminTestsFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task SignInPage_ForAnonymousUser_ReturnsOk()
    {
        // Arrange
        var url = "/signin";
        var applicationFactory = _fixture.ApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/Index")]
    [InlineData("/ManageData")]
    [InlineData("/ManageUsers")]
    public async Task ProtectedPages_ForAnonymousUser_RedirectsToSignin(string url)
    {
        // Arrange
        var expectedUrl = "/signin";
        var applicationFactory = _fixture.ApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.Should().NotBeNull();
        response.RequestMessage?.RequestUri?.PathAndQuery.Should().BeEquivalentTo(expectedUrl);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
