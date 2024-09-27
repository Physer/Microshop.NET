using FluentAssertions;
using System.Net;
using Tests.Builders;

namespace Tests;

public class HttpClientBuilderTests
{
    [Fact]
    public void BuildHttpClient_WithDefaultProperties_ReturnsHttpClientWithProperties()
    {
        // Arrange
        var httpClientBuilder = HttpClientBuilderBuilder.Build();

        // Act
        var httpClient = httpClientBuilder.BuildHttpClient();

        // Assert
        httpClient.Should().NotBeNull();
        httpClientBuilder.StatusCode.Should().Be(HttpStatusCode.OK);
        httpClientBuilder.ResponseContent.Should().NotBeNull();
        httpClientBuilder.Headers.Should().BeEmpty();
    }
}
