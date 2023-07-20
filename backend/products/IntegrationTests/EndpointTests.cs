using FluentAssertions;
using MassTransit.Testing;
using Messaging.Messages;
using System.Net;
using Xunit;

namespace IntegrationTests;

public class EndpointTests : IClassFixture<EndpointTestsFixture>
{
    private readonly EndpointTestsFixture _fixture;

    public EndpointTests(EndpointTestsFixture fixture) => _fixture = fixture;

    [Fact]
    public void GenerateProducts_WithoutOptions_ThrowsException()
    {
        // Arrange
        // Act
        var exception = Record.Exception(() => new WebApplicationFactoryWithServicebus<Program>(default).CreateClient());

        // Assert
        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public async Task GenerateProducts_ReturnsAccepted()
    {
        // Arrange
        var client = new WebApplicationFactoryWithServicebus<Program>(_fixture.ServicebusOptions).CreateClient();

        // Act
        var response = await client.PostAsync("/products", default);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
    }

    [Fact]
    public async Task GenerateProducts_PublishesMessage()
    {
        // Arrange
        var applicationFactory = new WebApplicationFactoryWithServicebus<Program>(_fixture.ServicebusOptions);
        var client = applicationFactory.CreateClient();
        var testHarness = applicationFactory.Services.GetTestHarness();

        // Act
        _ = await client.PostAsync("/products", default);

        // Assert
        (await testHarness.Published.Any<ProductsGenerated>()).Should().BeTrue();
    }
}
