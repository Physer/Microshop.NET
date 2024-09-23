using FluentAssertions;
using Tests.Builders;

namespace Tests;

public class HttpClientBuilderTests
{
    [Fact]
    public void BuildHttpClient_WithDefaultProperties_ReturnsHttpClient()
    {
        // Arrange
        var httpClientBuilder = HttpClientBuilderBuilder.Build();

        // Act
        var httpClient = httpClientBuilder.BuildHttpClient();

        // Assert
        httpClient.Should().NotBeNull();
    }
}
