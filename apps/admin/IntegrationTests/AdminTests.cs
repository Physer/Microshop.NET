using FluentAssertions;
using System.Net;
using Xunit;

namespace IntegrationTests;

public class AdminTests : IClassFixture<AdminTestsFixture>
{
    private readonly AdminTestsFixture _fixture;

    public AdminTests(AdminTestsFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task SignInPage_ForAnonymousUser_ReturnsOk()
    {
        // Arrange
        var applicationFactory = _fixture.ApplicationFactory!;
        var client = applicationFactory.CreateClient();

        // Act
        var response = await client.GetAsync("/signin");

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
