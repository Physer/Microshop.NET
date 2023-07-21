using FluentAssertions;
using MassTransit.Testing;
using Messaging.Messages;
using System.Net;
using Xunit;

namespace IntegrationTests;

public class ApiTests : IClassFixture<ApiTestsFixture>
{
    private readonly ApiTestsFixture _fixture;

    public ApiTests(ApiTestsFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task GenerateProducts_PublishesMessage_AndReturnsAccepted()
    {
        // Arrange
        var client = _fixture.ApplicationFactory.CreateClient();
        var testHarness = _fixture.ApplicationFactory.Services.GetTestHarness();

        // Act
        var response = await client.PostAsync("/products", default);

        // Assert
        response.Should().NotBeNull();
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);
        (await testHarness.Published.Any<GenerateProducts>()).Should().BeTrue();
    }
}
