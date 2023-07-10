using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace IntegrationTests;

public class EndpointTests
{
    [Fact]
    public async Task HealthEndpoint_ReturnsOkAndMessage()
    {
        // Arrange
        var client = new WebApplicationFactory<Program>().CreateClient();

        // Act
        var response = await client.GetAsync("/");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();
        content.Should().Be("Hello World!");
    }
}
