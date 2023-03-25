using FluentAssertions;
using RabbitMQ.Client.Exceptions;
using Tests.Builders;
using Xunit;

namespace Tests;

public class ProductsClientTests
{
    [Fact]
    public async Task GetProductsAsync_WhenBrokerIsUnreachable_ThrowsBrokerUnreachableException()
    {
        // Arrange
        var productsClient = new ProductsClientBuilder()
            .WithRequestClientUnreachable()
            .Build();

        // Act
        var result = async () => await productsClient.GetProductsAsync();

        // Assert
        await result.Should().ThrowAsync<BrokerUnreachableException>();
    }
}
