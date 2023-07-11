using AutoFixture.Xunit2;
using Domain;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UnitTests.Builders;
using Xunit;

namespace UnitTests;

public class ProductsGeneratedConsumerTest
{
    [Theory]
    [AutoData]
    public async Task Consume_WithMessage_CallsLoggerAndIndexingServiceWithProducts(IEnumerable<Product> products)
    {
        // Arrange
        var productsGeneratedConsumerBuilder = new ProductsGeneratedConsumerBuilder()
            .WithReceivingProducts(products)
            .WithMessage(Guid.NewGuid());
        var productsGeneratedConsumer = productsGeneratedConsumerBuilder.Build();

        // Act
        await productsGeneratedConsumer.Consume(productsGeneratedConsumerBuilder._consumeContext!);

        // Assert
        await productsGeneratedConsumerBuilder._indexingService.Received(1).IndexProductsAsync(products);
        productsGeneratedConsumerBuilder._logger.ReceivedWithAnyArgs(1).LogInformation("Logging with argument: {argument}", new[] { "argument" });
    }

    [Fact]
    public async Task Consumer_WithoutValidMessageData_CallsLoggerAndIndexingServiceWithEmptyCollection()
    {
        // Arrange
        var productsGeneratedConsumerBuilder = new ProductsGeneratedConsumerBuilder();
        var productsGeneratedConsumer = productsGeneratedConsumerBuilder.Build();

        // Act
        await productsGeneratedConsumer.Consume(productsGeneratedConsumerBuilder._consumeContext!);

        // Assert
        await productsGeneratedConsumerBuilder._indexingService.Received(1).IndexProductsAsync(Enumerable.Empty<Product>());
        productsGeneratedConsumerBuilder._logger.ReceivedWithAnyArgs(1).LogInformation("Logging with argument: {argument}", new[] { "argument" });
    }

    [Fact]
    public async Task Consumer_WithoutConsumeContext_CallsLoggerAndIndexingServiceWithEmptyCollection()
    {
        // Arrange
        var productsGeneratedConsumerBuilder = new ProductsGeneratedConsumerBuilder().WithoutConsumeContext();
        var productsGeneratedConsumer = productsGeneratedConsumerBuilder.Build();

        // Act
        await productsGeneratedConsumer.Consume(productsGeneratedConsumerBuilder._consumeContext!);

        // Assert
        await productsGeneratedConsumerBuilder._indexingService.Received(1).IndexProductsAsync(Enumerable.Empty<Product>());
        productsGeneratedConsumerBuilder._logger.ReceivedWithAnyArgs(1).LogInformation("Logging with argument: {argument}", new[] { "argument" });
    }
}
