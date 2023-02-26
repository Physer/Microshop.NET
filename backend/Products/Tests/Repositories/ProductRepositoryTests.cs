using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using Persistence;

namespace Tests.Repositories;

public class ProductRepositoryTests
{
    [Theory, AutoData]
    public void GetProductById_WithExistingId_ReturnsProduct(ProductData productData, Product output)
    {
        // Arrange

        var productRepository = new ProductRepositoryBuilder()
            .WithProductData(new[] { productData })
            .WithMapDatabaseEntryReturns(productData, output)
            .Build();

        // Act
        var result = productRepository.GetProductById(productData.Id);

        // Assert
        result.Should().Be(output);

    }
}
