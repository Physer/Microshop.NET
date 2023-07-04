using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using MassTransit;
using ProductsClient.Contracts;
using RabbitMQ.Client.Exceptions;
using UnitTests.Builders;
using Xunit;

namespace UnitTests;

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

    [Fact]
    public async Task GetProductsAsync_WhenRequestClientTimesOut_ThrowsRequestTimeoutExceptionException()
    {
        // Arrange
        var productsClient = new ProductsClientBuilder()
            .WithRequestClientTimingOut()
            .Build();

        // Act
        var result = async () => await productsClient.GetProductsAsync();

        // Assert
        await result.Should().ThrowAsync<RequestTimeoutException>();
    }

    [Theory]
    [AutoData]
    public async Task GetProductsAsync_WithResponseFromBroker_MapsData(IEnumerable<ProductResponse> responseProducts, Product exampleProduct)
    {
        // Arrange
        var productsClient = new ProductsClientBuilder()
            .WithMapperMappingProduct(responseProducts.First(), exampleProduct)
            .WithRequestClientReturningProducts(responseProducts)
            .Build();

        // Act
        var result = await productsClient.GetProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().ContainItemsAssignableTo<Product>();
        result.Should().HaveCount(responseProducts.Count());
    }

    [Fact]
    public async Task GetProductsAsync_WithEmptyResponseFromBroker_ReturnsEmptyCollection()
    {
        // Arrange
        var productsClient = new ProductsClientBuilder()
            .WithRequestClientReturningProducts(null)
            .Build();

        // Act
        var result = await productsClient.GetProductsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
