using FluentAssertions;
using MassTransit.Testing;
using Messaging.Messages;
using Xunit;

namespace IntegrationTests;

public class ProductsServiceTests : IClassFixture<ProductsServiceTestsFixture>
{
    private readonly ProductsServiceTestsFixture _fixture;

    public ProductsServiceTests(ProductsServiceTestsFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task ReceivesGenerateProductsMessage_GeneratesProducts_PublishesProductsGenerated()
    {
        // Arrange
        var testHarness = _fixture.TestHarness!;

        // Act
        await testHarness.Start();
        await testHarness.Bus.Publish(new GenerateProducts());
        await testHarness.Stop();

        // Assert
        (await testHarness.Consumed.Any<GenerateProducts>()).Should().BeTrue();
        (await testHarness.Published.Any<ProductsGenerated>()).Should().BeTrue();
    }
}
