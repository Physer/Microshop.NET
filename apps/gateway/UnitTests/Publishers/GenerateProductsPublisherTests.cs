using Messaging.Messages;
using NSubstitute;
using Xunit;

namespace UnitTests.Publishers;

public class GenerateProductsPublisherTests
{
    [Fact]
    public async Task PublishMessage_CallsPublishEndpoint()
    {
        // Arrange
        var generateProductsPublisherBuilder = new GenerateProductsPublisherBuilder();
        var generateProductsPublisher = generateProductsPublisherBuilder
            .Build();

        // Act
        await generateProductsPublisher.PublishMessage();

        // Assert
        await generateProductsPublisherBuilder._publishEndpoint.Received(1).Publish(new GenerateProducts());
    }
}
