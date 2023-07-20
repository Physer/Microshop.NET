using FluentAssertions;
using MassTransit.Testing;
using Messaging;
using Messaging.Messages;
using Xunit;

namespace IntegrationTests;

public class GenerateProductsConsumerTests : IClassFixture<ConsumersFixture<GenerateProductsConsumer>>
{
    private readonly ConsumersFixture<GenerateProductsConsumer> _consumersFixture;

    public GenerateProductsConsumerTests(ConsumersFixture<GenerateProductsConsumer> consumersFixture) => _consumersFixture = consumersFixture;

    [Fact]
    public async Task GenerateProductsConsumer_ConsumerGenerateProductsMessageAndPublishesProductsGeneratedMessage()
    {
        // Arrange
        var testHarness = _consumersFixture.TestHarness;

        // Act
        await testHarness.Start();
        await testHarness.Bus.Publish(new GenerateProducts());
        await testHarness.Stop();

        // Assert
        (await testHarness.Consumed.Any<GenerateProducts>()).Should().BeTrue();
        (await testHarness.Published.Any<ProductsGenerated>()).Should().BeTrue();
    }
}
