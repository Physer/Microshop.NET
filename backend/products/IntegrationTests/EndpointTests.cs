using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace IntegrationTests;

public class EndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _applicationFactory;

    public EndpointTests(WebApplicationFactory<Program> applicationFactory)
    {
        _applicationFactory = applicationFactory;
    }

    [Fact]
    public async Task GenerateProducts_ReturnsOkAndMessageId()
    {
        // Arrange
        var client = _applicationFactory.CreateClient();

        // Act
        var response = await client.PostAsync("/products", default);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();
        Guid.TryParse(content, out var parsedGuid).Should().BeTrue();
        parsedGuid.Should().NotBe(Guid.Empty);
    }
}
