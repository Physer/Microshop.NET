using AutoFixture.Xunit2;
using Domain;
using Messaging.Messages;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace UnitTests.Consumers;

public class ProductsGeneratedConsumerTest
{
    [Theory]
    [AutoData]
    public async Task Consume_WithValidMessage_CallsLoggerAndGeneratesPricesAndPublishesMessage(IEnumerable<Price> prices, IEnumerable<Product> products)
    {
        // Arrange
        var productsGeneratedConsumerBuilder = new ProductsGeneratedConsumerBuilder();
        var productsGeneratedConsumer = productsGeneratedConsumerBuilder.WithConsumerContextHavingMessageWithProducts(products).Build();

        // Act
        await productsGeneratedConsumer.Consume(productsGeneratedConsumerBuilder._consumeContext);

        // Assert
        productsGeneratedConsumerBuilder._logger.ReceivedWithAnyArgs(1).LogInformation("Logging with argument: {argument}", "argument");
        productsGeneratedConsumerBuilder._priceGenerator.Received(1).GeneratePrices(productsGeneratedConsumerBuilder._consumeContext.Message.Products);
        await productsGeneratedConsumerBuilder._consumeContext.Publish<PricesGenerated>(new(prices));
    }
}
